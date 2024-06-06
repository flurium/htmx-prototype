# htmx-prototype
Prototype of using HTMX with Asp.Net Core instead of JavaScript UI framework

## Requirements

You need to implement certain behaviours (functionality) to see HTMX in usage:
- infinite scroll with loading of new items
- sending and validation forms, including images
- autosave on typing with debounce
- pasing new html into existing page
- using boost

## Implementation

We don't need authorization for now, let's not complicate.

Home page should contain form to add new item and list of all items.
List sholud be lazily loaded using infinite scroll. 
Form should have image field and several validations.
When product is successfully added into database htmx should add item for this html at the top of current list (without refreshing whole page and without loading all products from start).

Product Edit page should save changes on typing but with debounce (waiting untill stops typing).

As well there should be product details page.

## Tech Stack

- Asp.Net Core
- Pico CSS
- HTMX

Remove jQuery and Bootstrap and any other unnesessary libraries. 
Download htmx and pico css minified files and serve as static ones, don't forget version.
