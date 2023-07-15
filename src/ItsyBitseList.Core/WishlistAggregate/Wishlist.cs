﻿using ItsyBitseList.Core.Interfaces;
using Newtonsoft.Json;
using static ItsyBitseList.Core.WishlistAggregate.Wishlists.Commands.CreateWishlist;

namespace ItsyBitseList.Core.WishlistCollectionAggregate
{
    // create a Wishlist class according to the tests
    public class Wishlist : IRootEntity
    {
        public static string DefaultOwner = "Anonymous";
        public Wishlist() { } // required for Json deserialization
        private Wishlist(Guid id, string name, string owner)
        {
            Id = id;
            Name = name;
            Owner = owner;
        }
        [JsonProperty]
        public Guid Id { get; init; }
        [JsonProperty]
        public string Name { get; init; }
        [JsonProperty]
        public string Owner { get; init; } = DefaultOwner;
        private List<WishlistItem> items = new();

        public void SetItems(List<WishlistItem> items) => this.items = items;
        [JsonProperty]
        public IReadOnlyCollection<WishlistItem> Items { get => items.AsReadOnly(); init => items = value.ToList(); }

        public static Wishlist CreateWith(Guid id, string name, string? owner = null)
        {
            return new Wishlist(id, name, owner ?? Guid.NewGuid().ToString());
        }

        // a method to add items to the wishlist
        public Wishlist AddItem(Guid id, string item, Uri? link = null)
        {
            items.Add(new WishlistItem(id, this.Id, item, link));
            return this;
        }

        public void Remove(Guid id)
        {
            items.Remove(items.First(x => x.Id == id));
        }

        internal static Wishlist CreateWith(CreateWishlistCommand request)
        {
            return new Wishlist(Guid.NewGuid(), request.WishlistName, request.Owner ?? DefaultOwner);
        }
    }
}