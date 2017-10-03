# GOMC-Capstone-Senior-Project
> Need to ask client for team logo here!

This repository is a conglomeration of the source code and documentation pertaining to
the CSC 4996: GOMC Capstone Project. Within this repo will be a working version of our dynamic website as per our client's request. This is project is currently in development.

# Project Overview
The GOMC development team is focused on building a dynamic website for our client the GOMC Research & Development Team. The GOMC project is a simulation engine using the Monte Carlo method for vapor-liquid equilibria systems.

Currently the static website is forcing maintenance tasks to be done manually which is very error prone and time-consuming. The downloads page is not always up-to-date with what is available in the releases of their software. In addition, the software accepts input in raw text which is also error prone. The software users commonly write incorrect inputs which cause the program to crash.

Our solution for our client is to have a dynamic website making the downloads page automatically kept up to date as new releases are produced and the user would have automated validation on input data to insure that errors are addressed.

# Scope
The central goal of this website is to develop an underused website into a core tool for presentation of the research team, release management and data input.  

This project will consist of a full front-end redesign and implementation of a new website with a new toolchain chosen by the engineers of the development team. It will conform to it’s own code, performance and style standards as decided by the team with the client’s approval.

In addition to that the project will require integration with the restful Github API v3 in order to dynamically update announcements, new releases of code from the research team, an updated user manual, an updated tracking of the number of downloads and other components of the website at the client’s specification.

The website will allow users to input data required for the GOMC software, send it to an endpoint and receive an XML file consisting of the user's input in the form of a download. Lastly, the software itself will be modified by us to process and the simulation with the XML input saving the user time and giving them the convenience of installing the software and having their input file ready to go once they are up and running.

# Technical Details
>Will implement graphic here

### Front end
.Net asp
Razor
Pure.css
jQuery

### Back end
C#
Github Api V3
JSON
C/C++
Cmake

### Database
MySql

# Team Breakdown
Team Lead: Ahmed Taher
Team Members: Muamer Besic, Caleb Latimer
TA : Azam Peyvandipour

### Development Roles
-	UI / UX / Front-End / Presentation Lead : Caleb Latimer
-	Database / Back-End Lead : Ahmed Taher
-	QA / Documentation Lead : Muamer Besic  


### Terminonlogy

There are a couple of terms that will appear in our commit history and comments that readers should be made aware of:
- cwfh checking in to work from home
- cwfr checking in to work from remote

> This is version 2.2
