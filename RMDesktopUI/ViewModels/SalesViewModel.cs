using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
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

        private BindingList<ProductModel> _cart;

        public BindingList<ProductModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private int _itemQuantity;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public string SubTotal
        {
            get
            {
                // TODO - Replace with Calculation
                return "$0.00";
            }
        }

        public string Tax
        {
            get
            {
                // TODO - Replace with Calculation
                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                // TODO - Replace with Calculation
                return "$0.00";
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected
                // Make sure there is an item quantity

                return output;
            }
        }
        
        public void AddToCart()
        {

        }

        public void RemoveFromCart()
        {

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
