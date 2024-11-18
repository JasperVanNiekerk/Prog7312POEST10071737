# Prog7312POEST10071737

## Table of Contents

- [Project Overview](#project-overview)
- [Key Features](#key-features)
- [Cloning the Project](#cloning-the-project)
- [Dependencies and Requirements](#dependencies-and-requirements)
- [Compiling the Project](#compiling-the-project)
  - [Using Visual Studio](#using-visual-studio)
  - [Using Command-Line](#using-command-line)
- [Running the Project](#running-the-project)
- [Usage Instructions](#usage-instructions)
- [Contributing Guidelines](#contributing-guidelines)
- [License Information](#license-information)
- [Contact Information](#contact-information)

## Project Overview

**Prog7312POEST10071737** is a Windows Presentation Foundation (WPF) application designed to facilitate efficient issue reporting and management within an organization or community. The application allows users to submit, view, and manage issue reports across various categories, providing a streamlined interface for tracking and addressing concerns.

## Key Features

- **Issue Reporting:** Users can submit detailed reports including descriptions, locations, media files, and categorize issues for better management.
- **Real-Time Updates:** The application leverages data structures like AVL Trees and Binary Search Trees to manage and display reports efficiently.
- **User Subscription:** Users can subscribe to specific issues to receive updates and notifications regarding their status.
- **Responsive UI:** Built with ModernWpfUI, the application offers a clean and intuitive user interface with smooth navigation.
- **Email Notifications:** Integrated email service to send confirmation and updates to users upon report submissions and status changes.
- **Dynamic Visualization:** TreeView components dynamically display issue reports, allowing for easy navigation and management.

## Cloning the Project

To get a local copy of the project up and running, follow these steps:

### Prerequisites

- **Git:** Ensure you have Git installed on your machine. [Download Git](https://git-scm.com/downloads)
- **Visual Studio 2022:** The project is developed using Visual Studio. [Download Visual Studio](https://visualstudio.microsoft.com/downloads/)
- **.NET Framework 4.8:** Ensure .NET Framework 4.8 is installed. [Download .NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)

### Clone the Repository

1. **Open Terminal or Command Prompt**

2. **Navigate to Your Desired Directory**

   ```bash
   cd path/to/your/directory
   ```

3. **Clone the Repository**

   ```bash
   git clone https://github.com/your-username/Prog7312POEST10071737.git
   ```

4. **Navigate into the Project Directory**

   ```bash
   cd Prog7312POEST10071737
   ```

## Dependencies and Requirements

The project relies on several external libraries and packages to function correctly. Below is a list of dependencies along with their versions and installation instructions.

### NuGet Packages

- **CommonWin32**
  - **Version:** 2.1.0.3
  - **Documentation:** [CommonWin32 on NuGet](https://www.nuget.org/packages/CommonWin32/)
  
- **EASendMail**
  - **Version:** 7.9.2.6
  - **Documentation:** [EASendMail on NuGet](https://www.nuget.org/packages/EASendMail/)
  
- **HtmlAgilityPack**
  - **Version:** 1.11.67
  - **Documentation:** [HtmlAgilityPack on NuGet](https://www.nuget.org/packages/HtmlAgilityPack/)
  
- **Microsoft.Bcl.AsyncInterfaces**
  - **Version:** 8.0.0
  - **Documentation:** [Microsoft.Bcl.AsyncInterfaces on NuGet](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces/)
  
- **ModernWpfUI**
  - **Version:** 0.9.6
  - **Documentation:** [ModernWpfUI on NuGet](https://www.nuget.org/packages/ModernWpfUI/)

### .NET Framework

- **.NET Framework 4.8**
  - **Installation Guide:** [Install .NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)

## Compiling the Project

### Using Visual Studio

1. **Open the Solution**

   - Launch Visual Studio 2022.
   - Go to `File` > `Open` > `Project/Solution`.
   - Navigate to the cloned repository and select `Prog7312POEST10071737.sln`.

2. **Restore NuGet Packages**

   - Visual Studio typically restores NuGet packages automatically.
   - If not, right-click on the solution in the Solution Explorer and select `Restore NuGet Packages`.

3. **Build the Solution**

   - Go to `Build` > `Build Solution` or press `Ctrl + Shift + B`.
   - Ensure there are no build errors in the Error List.

### Using Command-Line

1. **Navigate to the Project Directory**

   ```bash
   cd Prog7312POEST10071737
   ```

2. **Restore NuGet Packages**

   ```bash
   nuget restore Prog7312POEST10071737.sln
   ```

3. **Build the Project**

   ```bash
   msbuild Prog7312POEST10071737.sln /p:Configuration=Release
   ```

4. **Troubleshooting Compilation Errors**

   - Ensure all dependencies are correctly installed.
   - Verify that the correct version of .NET Framework is targeted.
   - Check for any missing files or resources referenced in the project.

## Running the Project

### Using Visual Studio

1. **Start Debugging**

   - With the solution open in Visual Studio, press `F5` or go to `Debug` > `Start Debugging`.
   - The application should launch, displaying the main window.

2. **Release Mode**

   - To run without debugging, switch to `Release` mode:
     - Go to the toolbar and select `Release` from the dropdown menu.
     - Press `Ctrl + F5` or go to `Debug` > `Start Without Debugging`.

### Executable

After building the project, you can run the executable directly:

1. **Navigate to the Output Directory**

   ```bash
   cd bin\Release\
   ```

2. **Run the Executable**

   ```bash
   Prog7312POEST10071737.exe
   ```

## Usage Instructions

### Submitting an Issue Report

1. **Launch the Application**

2. **Navigate to Report Issues**

   - Click on the `Report Issues` button in the main window.

3. **Navigate to the report form**

   - Click on the `See an Issue Report it`

4.  **Fill in the Report Form**

   - **Description:** Enter a detailed description of the issue.
   - **Location:** Specify the location related to the issue.
   - **Category:** Select the appropriate category from the dropdown menu.
   - **Media Files:** Upload any relevant media files (images, documents) related to the issue.
   - **Report Confirmation:** Optionally, check the confirmation box to receive an email notification.

5. **Submit the Report**

   - Click the `Submit` button to send your report.
   - A confirmation message will appear if the submission is successful.

### Viewing Issue Reports

1. **Navigate to Report Views**

   - Click on the `See an Issue Report it` button to toggle between the report form and the reports list.

2. **View Reports**

   - Reports are displayed in a TreeView, organized by categories.
   - Expand each report to view details such as ID, Description, Location, Media Files, Status, Category, and Subscribed Users.

3. **Subscribe to a Report**

   - Users can subscribe to specific reports to receive updates.
   - Subscribed users are listed under each report.



### Screenshots

![Main Window](https://github.com/user-attachments/assets/0a6a5fca-7e00-4bec-8b9f-ce703b2f2a98)
*Main window showcasing the reporting interface.*

![Report Form](https://github.com/user-attachments/assets/e6de60fb-e365-4552-8276-f8992d7cc149)
*Report form for submitting new issue reports.*

![Reports TreeView](https://github.com/user-attachments/assets/c1c13639-e9ee-49d4-b915-823aecd2313e)
*TreeView displaying submitted issue reports.*

## License Information

This project is licensed under the [MIT License](LICENSE).

## Contact Information

If you encounter any issues or have questions regarding the project, feel free to reach out:

- **Email:** jaspervanniekerk1111@gmail.com
- **GitHub Issues:** [Open an Issue](https://github.com/your-username/Prog7312POEST10071737/issues)

---

© 2024 Prog7312POEST10071737. All rights reserved.
