# WorkFlowApp

## Description

WorkFlowApp is a .NET application designed to manage and execute workflows based on predefined job files. The application reads workflow and job definitions from specified text files, processes them, and executes the corresponding tasks in an efficient manner. This tool is useful for automating repetitive tasks, ensuring consistency, and improving productivity in various business processes.

This project was developed as part of the **SE307** - **Concepts of Object-Oriented Programming** course.

## Features

- Read workflow definitions from a file
- Read job definitions from a file
- Execute workflows based on the job definitions
- Log the execution process and results

## Requirements

- .NET 8.0 SDK

## Usage

To run the WorkFlowApp, use the following command:

```sh
dotnet run src/Data/workflowFile.txt src/Data/jobFile.txt
```

Replace `src/Data/workflowFile.txt` and `src/Data/jobFile.txt` with the paths to your workflow and job definition files, respectively.
