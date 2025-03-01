# Optimizely Content Migration - Proof of Concept (POC)

## Overview
This repository contains a Proof of Concept (POC) for migrating any source content to Optimizely. The solution includes scripts and processes to efficiently transfer existing content, including metadata, assets, and links, while ensuring data integrity.

## Features
- Extracts blog content from the source system
- Transforms data into a format compatible with Optimizely
- Updates asset references
- Logs migration progress and errors

## Prerequisites
- .NET 6.0 or later
- Optimizely CMS setup
- Access to the source content database
- Required API keys and credentials (if applicable)

## Installation
1. Clone the repository:
   ```sh
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```sh
   cd optimizely-blog-migration
   ```
3. Restore dependencies:
   ```sh
   dotnet restore
   ```

## Usage
1. Update the configuration file (`appsettings.json`) with the source and target system details.
2. Run the migration script:
   ```sh
   dotnet run
   ```
3. Monitor the logs for any errors or warnings.

## Code Snippets
The repository includes key code snippets demonstrating:
- Content extraction logic ( Not Included)
- Data transformation methods ( Not Included )
- API interactions for Optimizely

## Contributing
If you would like to contribute, please fork the repository and submit a pull request with your changes.

## License
This project is licensed under the MIT License.

## Contact
For any queries or feedback, **[Contact me](https://www.wiselinjayajos.com/contact/).**
