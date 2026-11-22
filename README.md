# Woodcut Kerf Planner

Lay out cuts on stock lengths with saw kerf and report remaining offcuts.

![Sample run from Woodcut Kerf Planner](docs/demo.webp)

## Run

Install .NET 8 SDK and runtime. Run these commands from the repository root.

```sh
dotnet build --nologo
dotnet bin/Debug/net8.0/App.dll sample.json
```

Replace `sample.json` with a JSON file following the same structure. The app prints a JSON report to standard output.

All lengths use the same unit. The planner places longer pieces first and charges one kerf between adjacent pieces in its simplified layout.

## Verify

Run `python check.py` with Python 3.11 or newer after the build step when applicable. The check verifies the bundled scenario and confirms that an invalid input returns an error.
