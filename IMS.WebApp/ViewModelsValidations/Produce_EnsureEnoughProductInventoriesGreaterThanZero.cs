using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelsValidations
{
    public class Produce_EnsureEnoughProductInventoriesGreaterThanZero : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var produceViewModel = validationContext.ObjectInstance as ProduceViewModel;

            if (produceViewModel != null)
            {
                if (produceViewModel?.Product?.ProductInventories is null || produceViewModel.Product.ProductInventories.Count <= 0)
                {
                    var prodinvCount = produceViewModel?.Product?.ProductInventories.Count;
                    return new ValidationResult($"The product ({produceViewModel?.Product?.ProductName}) does not have enough Product Inventories associated. Inventories Associeted {( prodinvCount is null ? 0 : prodinvCount)} ",
                                                        new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
