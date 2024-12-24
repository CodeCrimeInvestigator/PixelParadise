# Use the official Nginx image
FROM nginx:alpine

# Copy the Nginx config file
COPY nginx.conf /etc/nginx/nginx.conf

# Mount the local image folder to the container's /usr/share/nginx/html/images
VOLUME /usr/share/nginx/html/images

# Expose the default HTTP port
EXPOSE 80