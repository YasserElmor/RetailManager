using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
        }

        // since we can't make the constructor asynchronous to fetch products on instantiation,
        // we'll have to use a lifecycle hook (OnViewLoaded) to trigger the LoadProducts method
        // as soon as the view's Loaded event is fired
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }
        private async Task LoadProducts()
        {
            var productsList = await _productEndpoint.GetAllAsync();
            Products = new BindingList<ProductModel>(productsList);
        }

        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private int _itemQuantity = 1;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal => CalculateSubTotal().ToString("C");

        private decimal CalculateSubTotal()
        {
            decimal subTotal = Cart.Aggregate(0.00M, (acc, item) => acc + (item.Product.RetailPrice * item.QuantityInCart));

            return subTotal;
        }

        public string Tax => CalculateTax().ToString("C");

        private decimal CalculateTax()
        {
            decimal taxRate = _configHelper.GetTaxRate() / 100;
            decimal taxAmount = Cart.Aggregate(0.00M, (acc, item) =>
            {
                if (item.Product.IsTaxable)
                    acc += item.Product.RetailPrice * item.QuantityInCart * taxRate;
                return acc;
            });

            return taxAmount;
        }

        public string Total => (CalculateSubTotal() + CalculateTax()).ToString("C");

        public bool CanAddToCart
        {
            get
            {
                // Make sure an item is selected
                // Make sure selected quantity is greater than zero
                // Make sure there is enough quantity in stock
                bool validItemQuantity = ItemQuantity > 0;
                bool validStockQuantity = SelectedProduct?.QuantityInStock >= ItemQuantity;
                bool output = validItemQuantity && validStockQuantity;

                return output;
            }
        }

        public void AddToCart()
        {
            CartItemModel item = Cart.FirstOrDefault(i => i.Product == SelectedProduct);


            if (item != null)
            {
                item.QuantityInCart += ItemQuantity;

                // we're removing and re-adding the item so that the cart is notified of the change in quantity to change the display
                Cart.Remove(item);
                Cart.Add(item);
            }
            else
            {
                item = new CartItemModel()
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity,
                };

                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public void RemoveFromCart()
        {

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected

                return output;
            }
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                // Make sure there is something in the cart

                return output;
            }
        }

        public void CheckOut()
        {

        }


    }
}
