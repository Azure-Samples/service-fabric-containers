# Use an official Node runtime as a base image
FROM node:6

# Set the working directory to /app
WORKDIR /app

# Copy the current directory contents into the container at /app
ADD . /app

# Make port 8000 available outside this container
EXPOSE 8000

# Define environment variable
ENV NAME World

# Run app.js when the container launches
CMD ["node", "app.js"]
