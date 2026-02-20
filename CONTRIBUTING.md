# Contributing to ColonesExchangeRate

Thank you for your interest in contributing! This guide will help you get started.

## Development Setup

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later
- [Node.js 22](https://nodejs.org/) or later (LTS)
- Git

### Clone the Repository
```bash
git clone https://github.com/dsanchezcr/ColonesExchangeRate.git
cd ColonesExchangeRate
```

### NuGet Package

```bash
# Restore dependencies
dotnet restore src/NuGet/ColonesExchangeRate.sln

# Build
dotnet build src/NuGet/ColonesExchangeRate.sln

# Run tests
dotnet test src/NuGet/ColonesExchangeRate.sln

# Run the sample console app
dotnet run --project src/NuGet/ClientConsole/ClientConsole.csproj
```

### npm Package

```bash
cd src/npm

# Install dependencies
npm install

# Run tests
npm test
```

## Project Structure

```
src/
├── npm/                          # npm package
│   ├── colonesexchangerate.mjs   # Main module
│   ├── colonesexchangerate.d.ts  # TypeScript definitions
│   ├── colonesexchangerate.test.js # Tests (node:test)
│   └── package.json
└── NuGet/                        # NuGet package
    ├── ColonesExchangeRate/       # Library project
    ├── ColonesExchangeRate.Tests/ # Test project (xUnit)
    └── ClientConsole/             # Sample console app
```

## Making Changes

1. **Fork** the repository and create a new branch from `main`.
2. Make your changes, ensuring:
   - Existing tests still pass.
   - New functionality includes tests.
   - Code follows the existing style and conventions.
3. **Test** your changes locally (both NuGet and npm if applicable).
4. **Submit** a pull request to `main` with a clear description of your changes.

## Code Style

- **C#**: Follow standard .NET conventions. Use XML documentation comments on public APIs.
- **JavaScript**: ESM modules, use `node:test` for tests, avoid unnecessary dependencies.

## Reporting Issues

Use [GitHub Issues](https://github.com/dsanchezcr/ColonesExchangeRate/issues) to report bugs or request features. For security vulnerabilities, please see [SECURITY.md](SECURITY.md).
