using AutoMapper;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Models;
using RMDesktopUI.ViewModels.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IMapper _mapper;
        private readonly IDisplayBox _displayBox;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint,
            IMapper mapper, IDisplayBox displayBox)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            _displayBox = displayBox;
        }

        // since we can't make the constructor asynchronous to fetch products on instantiation,
        // we'll have to use a lifecycle hook (OnViewLoaded) to trigger the LoadProducts method
        // as soon as the view's Loaded event is fired
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await LoadProducts();
        }

        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();

            SelectedCartItem = null;
            SelectedProduct = null;
            ItemQuantity = 1;

            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            try
            {
                var productsList = await _productEndpoint.GetAllAsync();
                var products = _mapper.Map<List<ProductDisplayModel>>(productsList);
                Products = new BindingList<ProductDisplayModel>(products);
            }
            catch (Exception ex)
            {
                await _displayBox.DisplayUnauthorizedMessageBoxAsync(ex);

                await TryCloseAsync();
            }
        }

        private BindingList<ProductDisplayModel> _products;

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private CartItemDisplayModel _selectedCartItem;
        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        public BindingList<CartItemDisplayModel> Cart
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
            CartItemDisplayModel item = Cart.FirstOrDefault(i => i.Product == SelectedProduct);


            if (item != null)
            {
                item.QuantityInCart += ItemQuantity;
            }
            else
            {
                item = new CartItemDisplayModel()
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
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanRemoveFromCart);
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
                SelectedCartItem.QuantityInCart -= 1;

            else
                Cart.Remove(SelectedCartItem);

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        public bool CanRemoveFromCart => SelectedCartItem != null;

        public bool CanCheckOut => Cart.Count > 0;

        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();

            Cart.ToList().ForEach(cartItem =>
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = cartItem.Product.Id,
                    Quantity = cartItem.QuantityInCart
                });
            });

            await _saleEndpoint.PostSale(sale);

            await ResetSalesViewModel();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
    }
}
