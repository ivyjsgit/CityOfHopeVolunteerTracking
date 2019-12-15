# This is the base image which determines from which Docker image the container should derive.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine
# Copy the files and folders from current directory to "app" directory
COPY . /app
 
# Set the working directory as "app" directory
WORKDIR /app
 
RUN dotnet restore && dotnet build
 
# Build the application
# Expose a port number from the container to outside world
EXPOSE 5000
EXPOSE 5001

 
# Determine an entry point of the application.
ENTRYPOINT ["dotnet", "run"]