using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Sunday.ContentDelivery.Core.Models;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement;
using Sunday.ContentManagement.Extensions;
using Sunday.ContentManagement.Models;
using Sunday.Core.Extensions;

namespace Sunday.Portfolio.Project.Components
{
    [ViewComponent(Name = "ExperiencesList")]
    public class ExperiencesListComponent : ViewComponent
    {
        private readonly IContentReader _contentReader;

        public ExperiencesListComponent(IContentReader contentReader)
        {
            _contentReader = contentReader;
        }

        public async Task<IViewComponentResult> InvokeAsync(RenderingParameters parameters)
        {
            if (parameters.Datasource == null) return new ContentViewComponentResult(string.Empty);
            var childs = parameters.Datasource.IdListValue("Experiences");
            var experiences = new List<Content>();
            foreach (var child in childs)
            {
                var exp = await _contentReader.GetContent(child);
                exp.IfSome(e => experiences.Add(e));
            }
            var heading = parameters.Datasource.TextValue("Heading");
            return View(new ExperiencesListComponent.ExperienceListBlock
            (heading!, experiences.Select(experience =>
                new ExperienceListBlock.Experience(experience.TextValue("Timerange") ?? string.Empty,
                    experience.TextValue("Position") ?? string.Empty,
                    experience.TextValue("Description") ?? string.Empty)).ToArray()));
        }

        public class ExperienceListBlock
        {
            public ExperienceListBlock(string heading, Experience[] experiences)
            {
                Experiences = experiences;
                Heading = heading;
            }

            public string Heading { get; set; }
            public Experience[] Experiences { get; }
            public class Experience
            {
                public string TimeRange { get; }
                public string Position { get; }
                public string Description { get; }

                public Experience(string timeRange, string position, string description)
                {
                    TimeRange = timeRange;
                    Position = position;
                    Description = description;
                }
            }
        }
    }
}
