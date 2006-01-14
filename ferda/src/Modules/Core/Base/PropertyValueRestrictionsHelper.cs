using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes
{
    /// <summary>
    /// Provides some static functions for testing rightness of 
    /// box module`s properties values against the restrictions.
    /// See <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetPropertyRestrictions(System.String)"/>
    /// and <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetPropertyRegexp(System.String)"/>
    /// </summary>
    public static class PropertyValueRestrictionsHelper
    {
        /// <summary>
        /// Tests if <c>propertyValue</c> of property named <c>propertyName</c>
        /// meets the requirements. Requirements are given by
        /// <see cref="T:Ferda.Modules.Serializer.BoxSerializer.Restriction">restrictions</see>.
        /// Iff <c>propertyValue</c> doesn`t meet the requirements than 
        /// <see cref="T:Ferda.Modules.BadValueError"/> is thrown.
        /// </summary>
        /// <param name="boxInfo">The box info</param>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="propertyValue">A long value of property.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">
        /// This execption is thrown iff <c>propertyValue</c> doesn`t satisfy restrictions.
        /// </exception>
        public static void TryIsIntegralPropertyCorrect(IBoxInfo boxInfo, string propertyName, long propertyValue)
        {
            List<Restriction> restrictions = boxInfo.GetPropertyRestrictions(propertyName);
            BadValueError possibleException = new BadValueError();
            foreach (Restriction restriction in restrictions)
            {
                if (restriction.integral.Length == 0)
                    continue;
                if (restriction.min)
                {
                    if (restriction.including)
                    {
                        if (!(restriction.integral[0] <= propertyValue))
                        {
                            possibleException.restrictionType = restrictionTypeEnum.Minimum;
                            throw possibleException;
                        }
                    }
                    else
                    {
                        if (!(restriction.integral[0] < propertyValue))
                        {
                            possibleException.restrictionType = restrictionTypeEnum.Minimum;
                            throw possibleException;
                        }
                    }
                }
                else
                {
                    if (restriction.including)
                    {
                        if (!(restriction.integral[0] >= propertyValue))
                        {
                            possibleException.restrictionType = restrictionTypeEnum.Maximum;
                            throw possibleException;
                        }
                    }
                    else
                    {
                        if (!(restriction.integral[0] > propertyValue))
                        {
                            possibleException.restrictionType = restrictionTypeEnum.Maximum;
                            throw possibleException;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tests if <c>propertyValue</c> of property named <c>propertyName</c>
        /// meets the requirements. Requirements are given by
        /// <see cref="T:Ferda.Modules.Serializer.BoxSerializer.Restriction">restrictions</see>.
        /// Iff <c>propertyValue</c> doesn`t meet the requirements than 
        /// <see cref="T:Ferda.Modules.BadValueError"/> is thrown.
        /// </summary>
        /// <param name="boxInfo">The box info</param>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="propertyValue">A double value of property.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">
        /// This execption is thrown iff <c>propertyValue</c> doesn`t satisfy restrictions.
        /// </exception>
        public static void TryIsFloatingPropertyCorrect(IBoxInfo boxInfo, string propertyName, double propertyValue)
        {
            List<Restriction> restrictions = boxInfo.GetPropertyRestrictions(propertyName);
            BadValueError possibleException = new BadValueError();
            foreach (Restriction restriction in restrictions)
            {
                if (restriction.floating.Length == 0)
                    continue;
                if (restriction.min)
                {
                    if (restriction.including)
                    {
                        if (!(restriction.floating[0] <= propertyValue))
                        {
                            possibleException.restrictionType = restrictionTypeEnum.Minimum;
                            throw possibleException;
                        }
                    }
                    else
                    {
                        if (!(restriction.floating[0] < propertyValue))
                        {
                            possibleException.restrictionType = restrictionTypeEnum.Minimum;
                            throw possibleException;
                        }
                    }
                }
                else
                {
                    if (restriction.including)
                    {
                        if (!(restriction.floating[0] >= propertyValue))
                        {
                            possibleException.restrictionType = restrictionTypeEnum.Maximum;
                            throw possibleException;
                        }
                    }
                    else
                    {
                        if (!(restriction.floating[0] > propertyValue))
                        {
                            possibleException.restrictionType = restrictionTypeEnum.Maximum;
                            throw possibleException;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tests if <c>propertyValue</c> of property named <c>propertyName</c>
        /// meets the requirements. Requirements are given by
        /// <see cref="T:Ferda.Modules.Serializer.BoxSerializer.Restriction">restrictions</see>.
        /// Iff <c>propertyValue</c> doesn`t meet the requirements than 
        /// <see cref="T:Ferda.Modules.BadValueError"/> is thrown.
        /// </summary>
        /// <param name="boxInfo">The box info</param>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="propertyValue">A string value of property.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">
        /// This execption is thrown iff <c>propertyValue</c> doesn`t satisfy restrictions.
        /// </exception>
        public static void TryIsStringPropertyCorrect(IBoxInfo boxInfo, string propertyName, string propertyValue)
        {
            string regexp = boxInfo.GetPropertyRegexp(propertyName);
            if (!String.IsNullOrEmpty(regexp))
            {
                if (!
                    System.Text.RegularExpressions.Regex.IsMatch(propertyValue,
                                                                regexp))
                {
                    BadValueError possibleException = new BadValueError();
                    possibleException.restrictionType = restrictionTypeEnum.Regexp;
                    throw possibleException;
                }
            }
            SelectString[] selectValues = boxInfo.GetPropertyFixedOptions(propertyName);
            if (selectValues.Length > 0)
            {
                bool inOptions = false;
                foreach (SelectString selectString in selectValues)
                {
                    if (selectString.name == propertyValue)
                    {
                        inOptions = true;
                        break;
                    }
                }
                if (!inOptions)
                {
                    BadValueError possibleException = new BadValueError();
                    possibleException.restrictionType = restrictionTypeEnum.NotInSelectOptions;
                    throw possibleException;
                }
            }
        }

        /// <summary>
        /// Tests if <c>propertyValue</c> of property named <c>propertyName</c>
        /// meets the requirements. Requirements are given by
        /// <see cref="T:Ferda.Modules.Serializer.BoxSerializer.Restriction">restrictions</see>.
        /// Iff <c>propertyValue</c> doesn`t meet the requirements than 
        /// <see cref="T:Ferda.Modules.BadValueError"/> is thrown.
        /// </summary>
        /// <param name="boxInfo">The box info</param>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="propertyValue">A <see cref="T:Ferda.Modules.DateTimeT"/> value of property.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">
        /// This execption is thrown iff <c>propertyValue</c> doesn`t satisfy restrictions.
        /// </exception>
        public static void TryIsDateTimePropertyCorrect(IBoxInfo boxInfo, string propertyName, DateTimeT propertyValue)
        {
            //TODO BODY (BoxInfo.TryIsDateTimePropertyCorrect())
        }

        /// <summary>
        /// Tests if <c>propertyValue</c> of property named <c>propertyName</c>
        /// meets the requirements. Requirements are given by
        /// <see cref="T:Ferda.Modules.Serializer.BoxSerializer.Restriction">restrictions</see>.
        /// Iff <c>propertyValue</c> doesn`t meet the requirements than 
        /// <see cref="T:Ferda.Modules.BadValueError"/> is thrown.
        /// </summary>
        /// <param name="boxInfo">The box info</param>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="propertyValue">A <see cref="T:Ferda.Modules.DateT"/> value of property.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">
        /// This execption is thrown iff <c>propertyValue</c> doesn`t satisfy restrictions.
        /// </exception>
        public static void TryIsDatePropertyCorrect(IBoxInfo boxInfo, string propertyName, DateT propertyValue)
        {
            //TODO BODY (BoxInfo.TryIsDatePropertyCorrect())
        }

        /// <summary>
        /// Tests if <c>propertyValue</c> of property named <c>propertyName</c>
        /// meets the requirements. Requirements are given by
        /// <see cref="T:Ferda.Modules.Serializer.BoxSerializer.Restriction">restrictions</see>.
        /// Iff <c>propertyValue</c> doesn`t meet the requirements than 
        /// <see cref="T:Ferda.Modules.BadValueError"/> is thrown.
        /// </summary>
        /// <param name="boxInfo">The box info</param>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="propertyValue">A <see cref="T:Ferda.Modules.TimeT"/> value of property.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">
        /// This execption is thrown iff <c>propertyValue</c> doesn`t satisfy restrictions.
        /// </exception>
        public static void TryIsTimePropertyCorrect(IBoxInfo boxInfo, string propertyName, TimeT propertyValue)
        {
            //TODO BODY (BoxInfo.TryIsTimePropertyCorrect())
        }
    }
}
