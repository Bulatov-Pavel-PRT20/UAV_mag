#!/usr/bin/env python

from dronekit import connect, VehicleMode, LocationGlobalRelative
import time
import math

# Connect to the vehicle (SITL usually on localhost)
vehicle = connect('127.0.0.1:14550', wait_ready=True)

# Set mode to GUIDED and arm
vehicle.mode = VehicleMode("GUIDED")
vehicle.armed = True

while not vehicle.armed:
    print("Waiting for arming...")
    time.sleep(1)

# Take off to 5 meters
target_altitude = 5
vehicle.simple_takeoff(target_altitude)

# Wait until the drone reaches desired altitude
while True:
    alt = vehicle.location.global_relative_frame.alt
    print("Current altitude: %.1f m" % alt)
    if alt >= target_altitude * 0.95:
        print("Target altitude reached")
        break
    time.sleep(1)

# Wait before forward movement
time.sleep(2)

# Function to calculate new location offset by meters
def get_location_metres(original_location, dNorth, dEast):
    earth_radius = 6378137.0
    dLat = dNorth / earth_radius
    dLon = dEast / (earth_radius * math.cos(math.pi * original_location.lat / 180))
    newlat = original_location.lat + (dLat * 180 / math.pi)
    newlon = original_location.lon + (dLon * 180 / math.pi)
    return LocationGlobalRelative(newlat, newlon, original_location.alt)

# Move 3 meters forward (north), stay at 5 meters altitude
start_location = vehicle.location.global_relative_frame
destination = get_location_metres(start_location, 3.0, 0.0)
destination.alt = 5.0  # enforce constant altitude

print("Flying forward 3 meters at 5 meters altitude...")
vehicle.simple_goto(destination)

# Wait for arrival
time.sleep(6)

# Optional: stop and hold position
print("Switching to LOITER to hold position")
vehicle.mode = VehicleMode("LOITER")
time.sleep(2)

vehicle.close()
print("Mission complete.")

