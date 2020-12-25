using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Sunday.ContentDelivery.Core.Models;
using Sunday.ContentDelivery.Core.Services;
using Sunday.ContentManagement.Extensions;

namespace Sunday.Portfolio.Project.Components
{
    [ViewComponent(Name = "AboutMe")]
    public class AboutMeViewComponent : ViewComponent
    {
        private readonly IContentReader _contentReader;

        public AboutMeViewComponent(IContentReader contentReader)
        {
            _contentReader = contentReader;
        }

        public async Task<IViewComponentResult> InvokeAsync(RenderingParameters parameters)
        {
            if (parameters.Datasource == null) return new ContentViewComponentResult(string.Empty);
            var heading = parameters.Datasource.TextValue("Heading")!;
            var subheading = parameters.Datasource.TextValue("Subheading")!;
            var description = parameters.Datasource.TextValue("Description")!;
            var skillIds = parameters.Datasource.IdListValue("Skills");
            var skills = new List<AboutMeBlock.Skill>();
            foreach (var skillId in skillIds)
            {
                var skill = await _contentReader.GetContent(skillId);
                skill.IfSome(sk =>
                {
                    skills.Add(new AboutMeBlock.Skill(sk.TextValue("Name")!,
                        int.Parse(sk.TextValue("Level")!)));
                });
            }
            return View(new AboutMeBlock(heading, subheading, skills.ToArray(), description));
        }

        public class AboutMeBlock
        {
            public AboutMeBlock(string heading, string subheading, Skill[] skills, string description)
            {
                Heading = heading;
                Subheading = subheading;
                Skills = skills;
                Description = description;
            }

            public string Heading { get; }
            public string Subheading { get; }
            public string Description { get;  }
            public Skill[] Skills { get; }

            public class Skill
            {
                public string Name { get; }
                public int Level { get; }

                public Skill(string name, int level)
                {
                    Name = name;
                    Level = level;
                }
            }
        }
    }
}
