using System;
using Xunit;
using TrinoClient.Model.Connector;

namespace TrinoClient.UnitTests
{
    public class ConnectorIdTests
    {
        [Fact]
        public void Constructor_ValidCatalogName_CreatesInstance()
        {
            // Arrange
            var catalogName = "test_catalog";

            // Act
            var connectorId = new ConnectorId(catalogName);

            // Assert
            Assert.Equal(catalogName, connectorId.CatalogName);
        }

        [Fact]
        public void Constructor_NullCatalogName_ThrowsException()
        {
            // Arrange
            string catalogName = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConnectorId(catalogName));
        }

        [Fact]
        public void Constructor_EmptyCatalogName_ThrowsException()
        {
            // Arrange
            var catalogName = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConnectorId(catalogName));
        }

        [Fact]
        public void ToString_ReturnsCatalogName()
        {
            // Arrange
            var catalogName = "test_catalog";
            var connectorId = new ConnectorId(catalogName);

            // Act
            var result = connectorId.ToString();

            // Assert
            Assert.Equal(catalogName, result);
        }

        [Fact]
        public void IsInternalSystemConnector_SystemTablesConnector_ReturnsTrue()
        {
            // Arrange
            var connectorId = new ConnectorId("$system@test_catalog");

            // Act
            var result = ConnectorId.IsInternalSystemConnector(connectorId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsInternalSystemConnector_InformationSchemaConnector_ReturnsTrue()
        {
            // Arrange
            var connectorId = new ConnectorId("$info_schema@test_catalog");

            // Act
            var result = ConnectorId.IsInternalSystemConnector(connectorId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsInternalSystemConnector_RegularConnector_ReturnsFalse()
        {
            // Arrange
            var connectorId = new ConnectorId("test_catalog");

            // Act
            var result = ConnectorId.IsInternalSystemConnector(connectorId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateInformationSchemaConnectorId_ValidConnectorId_CreatesCorrectly()
        {
            // Arrange
            var connectorId = new ConnectorId("test_catalog");

            // Act
            var infoSchemaConnector = ConnectorId.CreateInformationSchemaConnectorId(connectorId);

            // Assert
            Assert.Equal("$info_schema@test_catalog", infoSchemaConnector.CatalogName);
            Assert.True(ConnectorId.IsInternalSystemConnector(infoSchemaConnector));
        }

        [Fact]
        public void CreateSystemTablesConnectorId_ValidConnectorId_CreatesCorrectly()
        {
            // Arrange
            var connectorId = new ConnectorId("test_catalog");

            // Act
            var systemTablesConnector = ConnectorId.CreateSystemTablesConnectorId(connectorId);

            // Assert
            Assert.Equal("$system@test_catalog", systemTablesConnector.CatalogName);
            Assert.True(ConnectorId.IsInternalSystemConnector(systemTablesConnector));
        }
    }
}
