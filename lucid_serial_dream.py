import pygame
import time
import random
import serial
import threading
from collections import deque

# Constants
WINDOW_SIZE = 400
GRID_SIZE = 20
FPS = 60
MIN_COLOR_LIFETIME = 5  # Minimum color lifespan (seconds)
MAX_COLOR_LIFETIME = 10  # Maximum color lifespan (seconds)
SERIAL_PORT = '/dev/cu.usbmodem14201'  # Update as per your system
BAUD_RATE = 9600  # 9600 is default but we need speed.

# Validate if a string is a hex color code
def is_valid_hex_color(color):
    return len(color) == 7 and color.startswith("#") and all(c in "0123456789ABCDEFabcdef" for c in color[1:])

# Convert hex color to RGB
def hex_to_rgb(hex_color):
    return tuple(int(hex_color[i:i+2], 16) for i in (1, 3, 5))

# Linear interpolation between two colors
def lerp_color(c1, c2, t):
    return tuple(int(c1[i] + (c2[i] - c1[i]) * t) for i in range(3))


# Read color codes from the serial port and add to the queue
def read_serial_colors(ser, color_queue, stop_event):
    while not stop_event.is_set():
        if ser.in_waiting > 0:
            try:
                data = ser.readline().decode('utf-8').strip()
                if is_valid_hex_color(data):
                    color_queue.append(hex_to_rgb(data))
                    #print("color in queue: ", len(color_queue))
            except Exception as e:
                print(f"Error reading serial data: {e}")
        time.sleep(0.05)  # Small delay to prevent busy-waiting

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
        time.sleep(3)  # Give the Arduino some time to reset
    except serial.SerialException as e:
        print(f"Error: {e}")
        return

    # Create grid
    rows, cols = WINDOW_SIZE // GRID_SIZE, WINDOW_SIZE // GRID_SIZE
    grid = [[{"color": hex_to_rgb("#FFFFFF"), 
              "end_time": time.time() + random.uniform(MIN_COLOR_LIFETIME, MAX_COLOR_LIFETIME)} 
             for _ in range(cols)] for _ in range(rows)]

    # Color queue
    color_queue = deque()

    # Threading setup
    stop_event = threading.Event()
    serial_thread = threading.Thread(target=read_serial_colors, args=(ser, color_queue, stop_event))
    serial_thread.start()

    # Wait 20 seconds before starting the main loop
    print("Waiting 20 seconds before starting...")
    time.sleep(20)

    running = True

    while running:
        print("colors in queue: ", len(color_queue))
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False

        # Fill the background
        screen.fill((255, 255, 255))

        # Update grid colors
        current_time = time.time()
        for row in range(rows):
            for col in range(cols):
                cell = grid[row][col]

                # If the cell's lifetime has ended, assign a new color from the queue
                if current_time >= cell["end_time"]:
                    if color_queue:
                        cell["color"] = color_queue.popleft()
                    else:
                        cell["color"] = hex_to_rgb("#FFFFFF")  # Default to white if no colors in queue

                    # Set a new random lifetime
                    cell["end_time"] = current_time + random.uniform(MIN_COLOR_LIFETIME, MAX_COLOR_LIFETIME)

                # Get colors from adjacent cells (gradient effect)
                neighbors = [
                    grid[max(0, row - 1)][col]["color"],  # Top
                    grid[min(rows - 1, row + 1)][col]["color"],  # Bottom
                    grid[row][max(0, col - 1)]["color"],  # Left
                    grid[row][min(cols - 1, col + 1)]["color"],  # Right
                ]
                avg_color = tuple(sum(c[i] for c in neighbors) // len(neighbors) for i in range(3))
                cell["color"] = lerp_color(cell["color"], avg_color, 0.1)  # Smooth transition

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
    stop_event.set()
    serial_thread.join()
    ser.close()
    pygame.quit()

if __name__ == "__main__":
    main()
