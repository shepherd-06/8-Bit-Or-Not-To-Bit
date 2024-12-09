import time
import matplotlib.colors as mcolors
import pygame

def hex_to_rgb(hex_color):
    """Convert a hex color to an RGB tuple."""
    return mcolors.hex2color(hex_color)

def rgb_to_hex(rgb):
    """Convert an RGB tuple to a hex color."""
    return mcolors.to_hex(rgb)

def interpolate_color(start_rgb, end_rgb, t):
    """Interpolate between two RGB colors by a factor t (0 <= t <= 1)."""
    return tuple(start_rgb[i] + (end_rgb[i] - start_rgb[i]) * t for i in range(3))

def generate_color_steps(start_hex, end_hex, seconds):
    """
    Generate color steps from start_hex to end_hex over the given seconds.

    Parameters:
        start_hex (str): The starting hex color (e.g., "#ff0000").
        end_hex (str): The ending hex color (e.g., "#00ff00").
        seconds (float): Duration of the transition in seconds.

    Returns:
        List[str]: List of hex color steps.
    """
    fps = 60  # Frames per second
    total_frames = int(seconds * fps)
    start_rgb = hex_to_rgb(start_hex)
    end_rgb = hex_to_rgb(end_hex)

    steps = []
    for frame in range(total_frames + 1):
        t = frame / total_frames  # Interpolation factor (0 to 1)
        current_rgb = interpolate_color(start_rgb, end_rgb, t)
        steps.append(tuple(int(c * 255) for c in current_rgb))

    return steps

# Visualization with Pygame
if __name__ == "__main__":
    start_hex = "#ff0000"  # Red
    end_hex = "#00ff00"    # Green
    seconds = 3            # Duration in seconds

    color_steps = generate_color_steps(start_hex, end_hex, seconds)

    # Initialize Pygame
    pygame.init()
    screen = pygame.display.set_mode((400, 400))
    pygame.display.set_caption("Color Transition")

    clock = pygame.time.Clock()

    running = True
    frame = 0

    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False

        if frame < len(color_steps):
            color = color_steps[frame]
            screen.fill(color)
            frame += 1
        else:
            running = False

        pygame.display.flip()
        clock.tick(60)  # Limit to 60 FPS

    pygame.quit()
