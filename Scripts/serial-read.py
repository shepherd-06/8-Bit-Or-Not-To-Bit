import serial
import time

def read_serial_data():
    # Replace 'COM3' with the correct port for your system
    # For Linux/Mac, it might look like '/dev/ttyUSB0' or '/dev/ttyACM0'
    serial_port = '/dev/cu.usbmodem14101'
    baud_rate = 9600  # Match the baud rate in your Arduino sketch
    output_file = "arduino_data.txt"      # File to save the received data


    try:
        # Initialize the serial connection
        ser = serial.Serial(serial_port, baud_rate, timeout=1)
        print(f"Connected to {serial_port} at {baud_rate} baud.")

        # Give the Arduino some time to reset
        time.sleep(2)

        print("Reading data. Press Ctrl+C to stop.")
        with open(output_file, "a") as file:
            while True:
                # Read a line of input from the Arduino
                if ser.in_waiting > 0:
                    data = ser.readline().decode('utf-8').strip()
                    print(f"Received: {data}")
                    # Write data to the file
                    file.write(f"{data}\n")
                    file.flush()  # Ensure data is written to disk immediately

    except serial.SerialException as e:
        print(f"Error: {e}")
    except KeyboardInterrupt:
        print("\nExiting...")
    finally:
        # Close the serial connection
        if 'ser' in locals() and ser.is_open:
            ser.close()
            print("Serial connection closed.")

if __name__ == "__main__":
    read_serial_data()
