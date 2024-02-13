using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.Repository;
using WepPha2.Services;
using WepPha2.ViewModels;

namespace WepPha2.Controllers
{
    public class PurchaseDetailsController : Controller
    {
        private readonly IPurchaseDetailsRepository _purchaseDetailsRepository;
        private readonly IMedicineRepository _medicineRepository;

        public const string CartSessionKey = "CartId";
        public PurchaseDetailsController(IPurchaseDetailsRepository purchaseDetailsRepository, IMedicineRepository medicineRepository)
        {
            _purchaseDetailsRepository = purchaseDetailsRepository;
            _medicineRepository = medicineRepository;
        }
        public async Task<IActionResult> Details(int id)
        {
            var medicinesByIdPurchase = await _purchaseDetailsRepository.GetMedicinesByPurchaseId(id);
            return View(medicinesByIdPurchase);
        }

        public IActionResult AddToPurchase(int id, int count)
        {
            var cart = GetCart() ?? new Dictionary<int, int>();
            if (!cart.ContainsKey(id))
            {
                cart[id] = count;
                SaveCart(cart);
            }

            return RedirectToAction("Index", "Medicine");
        }

        private Dictionary<int, int>? GetCart()
        {
            var cart = Request.Cookies[CartSessionKey];
            return cart == null
                ? new Dictionary<int, int>()
                : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
        }
        public IActionResult ViewCart()
        {
            var cart = GetCart() ?? new Dictionary<int, int>();
            return View(cart);
        }
        public IActionResult CleanCart()
        {
            Response.Cookies.Delete(CartSessionKey);
            return RedirectToAction("Index","Purchase");
        }

        private void SaveCart(Dictionary<int, int> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            Response.Cookies.Append(CartSessionKey, cartJson);
        }
        public async Task<IActionResult> Create(int id)
        {          
            var cart = GetCart() ?? new Dictionary<int, int>();
            foreach (var item in cart)
            {     
                var purchasedmedicine = await _medicineRepository.GetMedicineById(item.Key);
                
                var purchaseDetails = new PurchaseDetails
                {
                    PurchasedMedicineId = item.Key,
                    Count = item.Value,
                    PurchaseId = id,
                    PurchasedMedicine =  purchasedmedicine
                };
                _purchaseDetailsRepository.Add(purchaseDetails);
                purchasedmedicine.Quantity = purchasedmedicine.Quantity - item.Value;
                _medicineRepository.Update(purchasedmedicine);
            }
            Response.Cookies.Delete(CartSessionKey);
            
            return RedirectToAction("Index", "Purchase");

        }
        public IActionResult DeleteFromCart(int id)
        {
            var cart = GetCart() ?? new Dictionary<int, int>();
            cart.Remove(id);
            SaveCart(cart);
            return RedirectToAction("ViewCart");
        }
        public async Task<IActionResult> UpdateCount(int itemId, int count)
        {
            var medicine = await _medicineRepository.GetMedicineById(itemId);
            if (count < medicine.Quantity)
            {
                var cart = GetCart() ?? new Dictionary<int, int>();
                cart[itemId] = count;
                SaveCart(cart);
                return RedirectToAction("ViewCart");
            }
            else
            {
                //return RedirectToAction("Error");
                return Problem();
            }

        }
    }
}
