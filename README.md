# W11ISOPatcher
Windows 11 ISO patcher.

![.NET Workflow](https://github.com/dongle-the-gadget/W11ISOPatcher/actions/workflows/dotnet.yml/badge.svg)

## Important notice about Version 1 Preview
The `appraiserres.dll` file hosted on this GitHub will be brought down on **September 5th, 2021** (GMT+0). On that date and beyond, you will have to [use a local `appraiserres.dll` file](#using-a-local-appraiserresdll-file) or the program will cease to work.

## Requirements
### Running
This program only officially supports Windows 10 May 2020 Update (version 2004) or newer.

I also recommend having at least 20 GB of free space.

### Building
Requires .NET 6 Preview Windows Desktop SDK and Visual Studio 2019 Preview or Visual Studio 2022 Preview.

### ISO
This program currently only works for x64 ISOs. I might add ARM64 support (but to be honest, pretty much all ARM64 devices meet system requirements).

## Usage
You will need:
  - A x64 Windows 11 ISO that you have read access to.
  - An empty folder that you have read and modify access to. (This will serve as our working folder)

First, launch the patcher and choose Next.

Then, choose the original ISO file, the working folder, and the final ISO path and choose Next.

**WARNING:** The final ISO file must not be in the working directory, otherwise the file will get deleted during cleanup.

Finally, wait for the patcher to finish its work.

**WARNING:** During the patching process **DO NOT** open or modify the working directory, this may disrupt the process.

### Using a local appraiserres.dll file

You can have the program use a local `appraiserres.dll` file by placing the file (under the name `appraiserres.dll`) in the same folder where the patcher is in.

**NOTE:** This program will always use the local file if found.

## Image
![Windows 11 ISO Patcher](https://user-images.githubusercontent.com/29563098/131250432-d3e8fbe2-1653-4cfe-9439-e6804080c70b.png)

## Support guidelines

They are [here](/docs/supportguidelines.md).

## Contributing guidelines
They are [here](/docs/CONTRIBUTING.md).

## Third party licenses
Third party licenses are available [here](/docs/thirdpartylicenses.md).

## Code of Conduct
This project follows [this Code of Conduct](/CODE_OF_CONDUCT.md).
