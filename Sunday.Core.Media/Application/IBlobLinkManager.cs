﻿using Sunday.Core.Media.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sunday.Core.Media.Application
{
    public interface IBlobLinkManager
    {
        string GetPreviewLink(ApplicationBlob blob, bool absolute = true);
    }
}
