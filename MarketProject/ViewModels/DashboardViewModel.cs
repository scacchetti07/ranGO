using MarketProject.Controllers;
using MarketProject.Models;

namespace MarketProject.ViewModels;

public class DashboardViewModel
{
    public Supply CurrentSupply { get; set; } = SupplyController.FindSupplyByName("ASSAI ATACADISTA");
}