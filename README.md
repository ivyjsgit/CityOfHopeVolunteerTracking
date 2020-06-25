# CityOfHopeVolunteerTracking [![BCH compliance](https://bettercodehub.com/edge/badge/ivyjsgit/CityOfHopeVolunteerTracking?branch=master)](https://bettercodehub.com/)
## What is this software?
------

This software is designed to be used by the Conway, AR non-profit City Of Hope. It is used to track volunteer hours, create reports containing total hours over timespans, calculate value of hours, and allow volunteers to view and print reports.

## Setting up encryption 

---

In order to set up encryption, you must enter your domain in Startup.cs and Program.cs. It will automatically get a SSL certificate using the Let's Encrypt service.

## Using Docker (Recommended)
---
#### Note: You MUST create a Docker Volume if you do not wish to lose all data once restarting a container.

### Using Docker pull
---
1. Run ```docker pull ivyjshdx/cohovolunteertracking```
2. Run ```docker run -p 5000:5000 -p 5001:5001 ivyjshdx/cohovolunteertracking```

### Building Locally
---
This is what should be used for development.

1. Clone the project.
2. ```docker build -t localreponame .```
3. ```docker run -p 5000:5000 -p 5001:5001 localreponame```                                     

## Using Dotnet
---
1. Run ```dotnet run```
