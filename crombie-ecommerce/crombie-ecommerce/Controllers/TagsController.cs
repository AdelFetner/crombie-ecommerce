﻿using crombie_ecommerce.Models.Entities;
using crombie_ecommerce.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace crombie_ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly TagService _tagsService;

        public TagsController(TagService tagsService)
        {
            _tagsService = tagsService;
        }

        // Get all tags
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagsService.GetAllTags();
            return Ok(tags);
        }

        // Get a tag by its ID
        [HttpGet("{tagId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetTagById(Guid tagId)
        {
            var tag = await _tagsService.GetTagById(tagId);
            if (tag == null)
            {
                return NotFound(new { message = "Tag not found" });
            }
            return Ok(tag);
        }

        // Create a new tag
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTag([FromBody] Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tags = await _tagsService.CreateTag(tag);
            return CreatedAtAction(nameof(GetTagById), new { tagId = tag.TagId }, tags);
        }

        // Delete a tag
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAndArchive(Guid id)
        {
            var success = await _tagsService.ArchiveMethod(id, "Unregistered");
            if (!success)
            {
                return NotFound(new { message = "Tag not found." });
            }
            return Ok(new { message = "Tag deleted successfully." });
        }

        // Associate a tag with a wishlist
        [HttpPost("{tagId:guid}/wishlist/{wishlistId:guid}")]
        [Authorize]
        public async Task<IActionResult> AddTagToWishlist(Guid tagId, Guid wishlistId)
        {
            var result = await _tagsService.AddTagToWishlist(tagId, wishlistId);
            if (!result)
            {
                return BadRequest(new { message = "Tag not found" });
            }
            return Ok(new { message = "Tag successfully associated with wishlist" });
        }

        // Disassociate a tag from a wishlist
        [HttpDelete("{tagId:guid}/wishlist")]
        [Authorize]
        public async Task<IActionResult> RemoveTagFromWishlist(Guid tagId)
        {
            var result = await _tagsService.RemoveTagFromWishlist(tagId);
            if (!result)
            {
                return BadRequest(new { message = "Tag not found" });
            }
            return Ok(new { message = "Tag successfully disassociated from wishlist" });
        }
    }
}
