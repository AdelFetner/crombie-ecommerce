﻿using crombie_ecommerce.Models;
using crombie_ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace crombie_ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly TagsService _tagsService;

        public TagsController(TagsService tagsService)
        {
            _tagsService = tagsService;
        }

        // Get all tags
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var tags = await _tagsService.GetAllTags();
            return Ok(tags);
        }

        // Get a tag by its ID
        [HttpGet("{tagId:guid}")]
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
        public async Task<IActionResult> CreateTag([FromBody] Tags tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTag = new Tags
            {
                TagId = Guid.NewGuid(),
                Name = tag.Name,
                Description = tag.Description,
                WishlistId = tag.WishlistId
            };

            var tags = await _tagsService.CreateTag(tag);
            return CreatedAtAction(nameof(GetTagById), new { tagId = tag.TagId }, tags);
        }

        // Delete a tag
        [HttpDelete("{tagId:guid}")]
        public async Task<IActionResult> DeleteTag(Guid tagId)
        {
            var tags = await _tagsService.DeleteTag(tagId);
            if (tags == null)
            {
                return NotFound(new { message = "Tag not found" });
            }
            return Ok(tags);
        }
    }
}
