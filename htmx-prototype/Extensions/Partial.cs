using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace htmx_prototype.Extensions
{
    public record Partial<T>(string Path)
    {
        public PartialViewResult Result(Controller controller, T model)
        {
            return controller.PartialView(Path, model);
        }

        public Task<IHtmlContent> Render(IHtmlHelper htmlHelper, T model)
        {
            return htmlHelper.PartialAsync(Path, model);
        }
    }
    public record Partial(string Path)
    {
        public PartialViewResult Result(Controller controller)
        {
            return controller.PartialView(Path);
        }

        public Task<IHtmlContent> Render(IHtmlHelper htmlHelper)
        {
            return htmlHelper.PartialAsync(Path);
        }
    }
}
