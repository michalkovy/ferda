// Sample.cs - Sample IceBox service
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2009 Martin Ralbovský
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using Ice;

namespace Ferda.Modules.Boxes.SemanticWeb
{
    /// <summary>
    /// Sample IceBox service
    /// </summary>
    public class Service : FerdaServiceI
    {
      /// <summary>
      /// Registers box to ice object adapter
      /// </summary>
      protected override void registerBoxes()
      {
          registerBox(PMMLBuilder.BoxInfo.typeIdentifier, new PMMLBuilder.BoxInfo());
          registerBox(SEWEBARPublisher.BoxInfo.typeIdentifier, new SEWEBARPublisher.BoxInfo());
      }
    }
}
