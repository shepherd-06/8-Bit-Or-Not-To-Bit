import pygame
import time
import random

# Constants
WINDOW_SIZE = 400
GRID_SIZE = 20
FPS = 60
COLOR_CHANGE_INTERVAL = 1  # 150 ms
NEW_COLOR_INTERVAL = 20       # 5 seconds
REVERT_TO_WHITE_INTERVAL = 50  # 10 seconds

# Load color codes from file and return as a list
def load_colors(file_path):
    with open(file_path, "r") as f:
        colors = [line.strip() for line in f.readlines() if line.startswith("#")]
    if not colors:
        raise ValueError("No valid color codes found in the file.")
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
    pygame.display.set_caption("Upgraded Lucid Gradient Grid")
    clock = pygame.time.Clock()

    # Load colors and initialize variables
    color_codes = load_colors("arduino_data.txt")
    color_index = 0  # To track the current position in the color file

    # Create grid
    rows, cols = WINDOW_SIZE // GRID_SIZE, WINDOW_SIZE // GRID_SIZE
    grid = [[{"color": hex_to_rgb("#FFFFFF"), "last_update": time.time()} for _ in range(cols)] for _ in range(rows)]

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
            for row in range(1, rows - 1):
                for col in range(1, cols - 1):
                    cell = grid[row][col]

                    # Revert to white after the interval
                    if current_time - cell["last_update"] >= REVERT_TO_WHITE_INTERVAL:
                        cell["color"] = hex_to_rgb("#FFFFFF")

                    # Check and replace white cells
                    if cell["color"] == hex_to_rgb("#FFFFFF"):
                        cell["color"] = hex_to_rgb(color_codes[color_index])
                        cell["last_update"] = current_time
                        color_index = (color_index + 1) % len(color_codes)

                    # Get colors from adjacent cells
                    c1 = grid[row][col]["color"]         # Top-left
                    c2 = grid[row][col + 1]["color"]     # Top-right
                    c3 = grid[row + 1][col]["color"]     # Bottom-left
                    c4 = grid[row + 1][col + 1]["color"]  # Bottom-right

                    # Interpolate using the four corners
                    t_x = random.random()  # Random interpolation factor for x
                    t_y = random.random()  # Random interpolation factor for y
                    top_color = lerp_color(c1, c2, t_x)
                    bottom_color = lerp_color(c3, c4, t_x)
                    cell["color"] = lerp_color(top_color, bottom_color, t_y)

            last_color_change = current_time

        # Add a new random gradient point every 5 seconds
        if current_time - last_new_color >= NEW_COLOR_INTERVAL:
            random_row = random.randint(1, rows - 2)
            random_col = random.randint(1, cols - 2)
            grid[random_row][random_col]["color"] = hex_to_rgb(color_codes[color_index])
            grid[random_row][random_col]["last_update"] = current_time
            color_index = (color_index + 1) % len(color_codes)
            last_new_color = current_time

        # Draw the grid
        for row in range(rows):
            for col in range(cols):
                pygame.draw.rect(
                    screen,
                    grid[row][col]["color"],
                    pygame.Rect(col * GRID_SIZE, row * GRID_SIZE, GRID_SIZE, GRID_SIZE),
                )

        # Update the display
        pygame.display.flip()
        clock.tick(FPS)

    pygame.quit()

if __name__ == "__main__":
    main()
