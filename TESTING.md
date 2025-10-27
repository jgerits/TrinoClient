# TrinoClient Testing

This project contains two types of tests:

## Unit Tests (TrinoClient.UnitTests)

Pure unit tests that can run without any external dependencies. These tests validate:
- TrinoClientSessionConfig validation and configuration
- TrinoHeader constants
- ExecuteQueryV1Request creation and validation
- QueryOptions validation
- Exception classes (TrinoException, TrinoWebException)
- Model classes (Column, TimeZoneKey)

### Running Unit Tests

```bash
# Run all unit tests
dotnet test TrinoClient.UnitTests/TrinoClient.UnitTests.csproj

# Run all tests excluding integration tests (recommended for CI/CD)
dotnet test --filter "Category!=Integration"
```

## Integration Tests (TrinoClient.Tests)

Integration tests that require a running Trino server. These tests are marked with `[Trait("Category", "Integration")]` and are excluded from automatic runs.

### Running Integration Tests

To run integration tests, you need to:

1. Have a running Trino server accessible at the configured host and port
2. Update the connection details in the test files if needed
3. Run the tests with the integration category filter:

```bash
# Run only integration tests
dotnet test --filter "Category=Integration"

# Run all tests (including integration tests)
dotnet test
```

## Continuous Integration

The CI/CD pipeline automatically runs only unit tests by using the filter `--filter "Category!=Integration"`. This ensures that builds don't fail due to the absence of a Trino server while still allowing integration tests to be run manually when needed.
