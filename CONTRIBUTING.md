# Contributing to TrinoClient

First off, thank you for considering contributing to TrinoClient! It's people like you that make TrinoClient such a great tool.

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible:

* **Use a clear and descriptive title** for the issue to identify the problem.
* **Describe the exact steps which reproduce the problem** in as much detail as possible.
* **Provide specific examples to demonstrate the steps**. Include links to files or GitHub projects, or copy/pasteable snippets.
* **Describe the behavior you observed after following the steps** and point out what exactly is the problem with that behavior.
* **Explain which behavior you expected to see instead and why.**
* **Include details about your configuration and environment:**
  * Which version of .NET are you using?
  * Which version of TrinoClient are you using?
  * Which version of Trino are you connecting to?

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, please include:

* **Use a clear and descriptive title** for the issue to identify the suggestion.
* **Provide a step-by-step description of the suggested enhancement** in as many details as possible.
* **Provide specific examples to demonstrate the steps**.
* **Describe the current behavior** and **explain which behavior you expected to see instead** and why.
* **Explain why this enhancement would be useful** to most TrinoClient users.

### Pull Requests

* Fill in the required template
* Do not include issue numbers in the PR title
* Follow the C# coding style (see below)
* Include thoughtfully-worded, well-structured tests
* Document new code with XML documentation comments
* End all files with a newline
* Avoid platform-dependent code

## Development Process

### Setting Up Your Development Environment

1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/YOUR-USERNAME/TrinoClient.git
   cd TrinoClient
   ```
3. Add the upstream repository:
   ```bash
   git remote add upstream https://github.com/jgerits/TrinoClient.git
   ```
4. Install .NET 8.0 SDK or later
5. Restore dependencies:
   ```bash
   dotnet restore
   ```
6. Build the solution:
   ```bash
   dotnet build
   ```
7. Run tests:
   ```bash
   dotnet test
   ```

### Making Changes

1. Create a new branch from `main`:
   ```bash
   git checkout -b feature/your-feature-name
   ```
2. Make your changes
3. Add tests for your changes
4. Ensure all tests pass:
   ```bash
   dotnet test
   ```
5. Ensure the build succeeds without warnings:
   ```bash
   dotnet build --configuration Release
   ```
6. Commit your changes using a descriptive commit message
7. Push your branch to GitHub:
   ```bash
   git push origin feature/your-feature-name
   ```
8. Create a Pull Request

### Commit Message Guidelines

* Use the present tense ("Add feature" not "Added feature")
* Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
* Limit the first line to 72 characters or less
* Reference issues and pull requests liberally after the first line
* Consider starting the commit message with an applicable emoji:
  * 🎨 `:art:` when improving the format/structure of the code
  * 🐛 `:bug:` when fixing a bug
  * 📝 `:memo:` when writing docs
  * 🚀 `:rocket:` when improving performance
  * ✅ `:white_check_mark:` when adding tests
  * 🔒 `:lock:` when dealing with security

## Coding Style

### General Guidelines

* Follow [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
* Use meaningful variable and method names
* Keep methods focused and small
* Use XML documentation comments for public APIs
* Prefer async/await over synchronous operations
* Use `var` for local variables when the type is obvious
* Use explicit types when the type is not obvious

### XML Documentation

All public classes, methods, and properties should have XML documentation:

```csharp
/// <summary>
/// Executes a SQL query against the Trino cluster.
/// </summary>
/// <param name="request">The query execution request.</param>
/// <param name="cancellationToken">Token to cancel the operation.</param>
/// <returns>The query execution response.</returns>
/// <exception cref="TrinoQueryException">Thrown when the query fails.</exception>
public async Task<ExecuteQueryV1Response> ExecuteQueryV1(
    ExecuteQueryV1Request request, 
    CancellationToken cancellationToken)
{
    // Implementation
}
```

### Testing

* Write unit tests for all new functionality
* Ensure tests are independent and can run in any order
* Use meaningful test names that describe what is being tested
* Follow the Arrange-Act-Assert pattern
* Mock external dependencies

Example:
```csharp
[Fact]
public async Task ExecuteQueryV1_WithValidRequest_ReturnsResults()
{
    // Arrange
    var config = new TrinoClientSessionConfig { Host = "localhost", Port = 8080 };
    var client = new TrinodbClient(config);
    var request = new ExecuteQueryV1Request("SELECT 1");

    // Act
    var response = await client.ExecuteQueryV1(request);

    // Assert
    Assert.NotNull(response);
}
```

### Error Handling

* Use specific exception types
* Provide meaningful error messages
* Document exceptions in XML comments
* Don't swallow exceptions without logging

## Project Structure

```
TrinoClient/
├── TrinoClient/              # Main library project
│   ├── Interfaces/           # Public interfaces
│   ├── Model/                # Data models
│   ├── Serialization/        # JSON serialization
│   └── TrinodbClient.cs      # Main client implementation
├── TrinoClientTests/         # Unit tests
└── README.md                 # Project documentation
```

## Release Process

Releases are automated through GitHub Actions:

1. Update version in `TrinoClient/TrinoClient.csproj`
2. Update `CHANGELOG.md` (if exists) or version notes in README
3. Merge changes to `main` branch
4. The CI/CD pipeline will automatically:
   - Build and test the project
   - Create a NuGet package
   - Publish to NuGet.org
   - Create a GitHub release

## Questions?

Feel free to open an issue with your question or reach out to the maintainers.

## License

By contributing to TrinoClient, you agree that your contributions will be licensed under the MIT License.
