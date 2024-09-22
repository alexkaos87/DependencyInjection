using Moq;
using ProductImporter.Core.Shared;
using ProductImporter.Core.Source;
using ProductImporter.Core.Target;
using ProductImporter.Core.Transformation;
using ProductImporter.Model;

namespace ProductImporter.Core.Tests
{
    [TestClass]
    public class ProductsManagerTests
    {
        private ProductsManager _manager;
        private Mock<IProductSource> _productSourceMock;
        private Mock<IProductTransformer> _productTransformerMock;
        private Mock<IProductTarget> _productTargetMock;
        private Mock<IImportStatistics> _importStatisticsMock;

        [TestInitialize]
        public void TestInit()
        {
            _productSourceMock = new();
            _productTransformerMock = new();
            _productTargetMock = new();
            _importStatisticsMock = new();

            _manager = new(_productSourceMock.Object, 
                _productTransformerMock.Object, 
                _productTargetMock.Object, 
                _importStatisticsMock.Object);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(5)]
        public async Task WhenItReadsNProductsFromSource_ThenItWritesNProductsToTarget(int numberOfProducts)
        {
            var productCounter = 0;

            _productSourceMock
                .Setup(x => x.hasMoreProducts())
                .Callback(() => productCounter++)
                .Returns(() => productCounter <= numberOfProducts);

            await _manager.RunAsync();

            _productTargetMock
                .Verify(x => x.AddProduct(It.IsAny<Product>()), Times.Exactly(numberOfProducts));
        }
    }
}