import pygame
import time
import random
import serial

# Constants
WINDOW_SIZE = 400
GRID_SIZE = 20
FPS = 60
COLOR_CHANGE_INTERVAL = 0.15  # 150 ms
REVERT_TO_WHITE_INTERVAL = 10  # 10 seconds
SERIAL_PORT = '/dev/cu.usbmodem14201'  # Update as per your system
BAUD_RATE = 28800 # 9600 is default but we need speed.

# Validate if a string is a hex color code
def is_valid_hex_color(color):
    return len(color) == 7 and color.startswith("#") and all(c in "0123456789ABCDEFabcdef" for c in color[1:])

# Convert hex color to RGB
def hex_to_rgb(hex_color):
    return tuple(int(hex_color[i:i+2], 16) for i in (1, 3, 5))

# Linear interpolation between two colors
def lerp_color(c1, c2, t):
    return tuple(int(c1[i] + (c2[i] - c1[i]) * t) for i in range(3))

# Read color codes from the serial port
def read_serial_color(ser):
    if ser.in_waiting > 0:
        try:
            data = ser.readline().decode('utf-8').strip()
            if is_valid_hex_color(data):
                print(data)
                return data
        except Exception as e:
            print(f"Error reading serial data: {e}")
    return None

# Main function
def main():
    # Initialize Pygame
    pygame.init()
    screen = pygame.display.set_mode((WINDOW_SIZE, WINDOW_SIZE))
    pygame.display.set_caption("Serial Gradient Grid")
    clock = pygame.time.Clock()

    # Initialize serial connection
    try:
        ser = serial.Serial(SERIAL_PORT, BAUD_RATE, timeout=1)
        print(f"Connected to {SERIAL_PORT} at {BAUD_RATE} baud.")
        time.sleep(2)  # Give the Arduino some time to reset
    except serial.SerialException as e:
        print(f"Error: {e}")
        return

    # Create grid
    rows, cols = WINDOW_SIZE // GRID_SIZE, WINDOW_SIZE // GRID_SIZE
    grid = [[{"color": hex_to_rgb("#FFFFFF"), "last_update": time.time()} for _ in range(cols)] for _ in range(rows)]

    running = True
    last_color_change = time.time()

    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False

        # Fill the background
        screen.fill((255, 255, 255))

        # Read new color from serial
        new_color = read_serial_color(ser)

        # Update grid colors
        current_time = time.time()
        if current_time - last_color_change >= COLOR_CHANGE_INTERVAL:
            for row in range(rows):
                for col in range(cols):
                    cell = grid[row][col]

                    # Revert to white after the interval
                    if current_time - cell["last_update"] >= REVERT_TO_WHITE_INTERVAL:
                        cell["color"] = hex_to_rgb("#FFFFFF")

                    # Randomly choose a cell to update
                    if new_color:
                        random_row = random.randint(0, rows - 1)
                        random_col = random.randint(0, cols - 1)
                        grid[random_row][random_col]["color"] = hex_to_rgb(new_color)
                        grid[random_row][random_col]["last_update"] = current_time

                    # Get colors from adjacent cells (gradient effect)
                    c1 = grid[max(0, row - 1)][max(0, col - 1)]["color"]  # Top-left
                    c2 = grid[max(0, row - 1)][min(cols - 1, col + 1)]["color"]  # Top-right
                    c3 = grid[min(rows - 1, row + 1)][max(0, col - 1)]["color"]  # Bottom-left
                    c4 = grid[min(rows - 1, row + 1)][min(cols - 1, col + 1)]["color"]  # Bottom-right

                    t_x = random.random()  # Random interpolation factor for x
                    t_y = random.random()  # Random interpolation factor for y
                    top_color = lerp_color(c1, c2, t_x)
                    bottom_color = lerp_color(c3, c4, t_x)
                    cell["color"] = lerp_color(top_color, bottom_color, t_y)

            last_color_change = current_time

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

    # Cleanup
    ser.close()
    pygame.quit()

if __name__ == "__main__":
    main()
