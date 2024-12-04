import pygame
import time
import random

# Constants
WINDOW_SIZE = 400
GRID_SIZE = 20
FPS = 60
COLOR_CHANGE_INTERVAL = 0.15  # 150 ms
NEW_COLOR_INTERVAL = 5  # 5 seconds

# Load color codes from file
def load_colors(file_path):
    with open(file_path, "r") as f:
        colors = [line.strip() for line in f.readlines() if line.startswith("#")]
    return colors

# Convert hex color to RGB
def hex_to_rgb(hex_color):
    return tuple(int(hex_color[i:i+2], 16) for i in (1, 3, 5))

# Linear interpolation between two colors
def lerp_color(c1, c2, t):
    return tuple(int(c1[i] + (c2[i] - c1[i]) * t) for i in range(3))

# Main function
def main():
    # Initialize Pygame
    pygame.init()
    screen = pygame.display.set_mode((WINDOW_SIZE, WINDOW_SIZE))
    pygame.display.set_caption("Lucid Dream Gradient Grid")
    clock = pygame.time.Clock()

    # Load colors
    color_codes = load_colors("arduino_data.txt")
    if not color_codes:
        print("No valid colors found in the file!")
        return

    # Create grid
    rows, cols = WINDOW_SIZE // GRID_SIZE, WINDOW_SIZE // GRID_SIZE
    grid = [[hex_to_rgb("#FFFFFF") for _ in range(cols)] for _ in range(rows)]

    # Timing for updates
    last_color_change = time.time()
    last_new_color = time.time()

    running = True
    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False

        # Fill the background
        screen.fill((255, 255, 255))

        # Update grid colors
        current_time = time.time()
        if current_time - last_color_change >= COLOR_CHANGE_INTERVAL:
            # Update inner grid gradients
            for row in range(1, rows - 1):
                for col in range(1, cols - 1):
                    # Get colors from adjacent cells
                    c1 = grid[row][col]          # Top-left
                    c2 = grid[row][col + 1]      # Top-right
                    c3 = grid[row + 1][col]      # Bottom-left
                    c4 = grid[row + 1][col + 1]  # Bottom-right

                    # Interpolate using the four corners
                    t_x = random.random()  # Random interpolation factor for x
                    t_y = random.random()  # Random interpolation factor for y
                    top_color = lerp_color(c1, c2, t_x)
                    bottom_color = lerp_color(c3, c4, t_x)
                    grid[row][col] = lerp_color(top_color, bottom_color, t_y)

            last_color_change = current_time

        # Add a new random gradient point every 5 seconds
        if current_time - last_new_color >= NEW_COLOR_INTERVAL:
            random_row = random.randint(1, rows - 2)
            random_col = random.randint(1, cols - 2)
            grid[random_row][random_col] = hex_to_rgb(random.choice(color_codes))
            last_new_color = current_time

        # Draw the grid
        for row in range(rows):
            for col in range(cols):
                pygame.draw.rect(
                    screen,
                    grid[row][col],
                    pygame.Rect(col * GRID_SIZE, row * GRID_SIZE, GRID_SIZE, GRID_SIZE),
                )

        # Update the display
        pygame.display.flip()
        clock.tick(FPS)

    pygame.quit()

if __name__ == "__main__":
    main()
