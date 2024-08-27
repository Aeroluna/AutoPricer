using HarmonyLib;
using UnityEngine;

namespace AutoPricer;

[HarmonyPatch]
public static class Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerNetwork), nameof(PlayerNetwork.PriceSetFromNumpad))]
    private static void AddMiddleClick(PlayerNetwork __instance, int productID)
    {
        if (!Input.GetMouseButtonDown(2))
        {
            return;
        }

        float inflation = ProductListing.Instance.tierInflation[ProductListing
            .Instance.productPrefabs[productID]
            .GetComponent<Data_Product>()
            .productTier];
        float basePrice =
            ProductListing.Instance.productPrefabs[productID].GetComponent<Data_Product>().basePricePerUnit *
            inflation;
        float newPrice = Mathf.Round(basePrice * 200f) / 100f;
        __instance.pPrice = newPrice;
        __instance.yourPriceTMP.text = ProductListing.Instance.ConvertFloatToTextPrice(newPrice);
        __instance.CmdPlayPricingSound();
        ProductListing.Instance.CmdUpdateProductPrice(productID, __instance.pPrice);
    }
}
