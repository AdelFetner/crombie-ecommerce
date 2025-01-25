using crombie_ecommerce.Models.Models.Entities;

namespace crombie_ecommerce.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<bool> AddProductToWishlist(Guid wishlistId, Guid productId);
        Task<List<Wishlist>> CreateWishlist(Wishlist wishlist);
        Task<List<Wishlist>> DeleteWishlist(Guid wishlistId);
        Task<List<Wishlist>> GetAllWishlists();
        Task<Wishlist?> GetWishlistById(Guid wishlistId);
        Task<bool> RemoveProductFromWishlist(Guid wishlistId, Guid productId);
    }
}