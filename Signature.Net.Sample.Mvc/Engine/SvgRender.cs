﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace Signature.Net.Sample.Mvc.Engine
{
    public class SvgRender
    {
        public Stream DrawSvgImage(string svgData)
        {
            MemoryStream outputImageStream = null;
            XDocument root = XDocument.Parse(svgData);
            IEnumerable<XElement> pathElements = root.Descendants("{http://www.w3.org/2000/svg}path");
            if (pathElements.Count() > 0)
            { // a drawn image
                using (Image image = new Bitmap(500, 500))
                {
                    using (Graphics graphics = Graphics.FromImage(image))
                    {
                        foreach (XElement pathElement in pathElements)
                        {
                            string drawingInstructionsString = pathElement.Attribute("d").Value;
                            Regex instructionRegex = new Regex(@"([CLM])([^CLM]*)[CLM$]");
                            var matches = instructionRegex.Matches(drawingInstructionsString);
                            double startX = 0, startY = 0;
                            double endX, endY;
                            string[] coordinates;
                            double controlPoint1X, controlPoint1Y, controlPoint2X, controlPoint2Y;
                            foreach (Match drawingInstructionMatch in matches)
                            {
                                string drawingInstruction = drawingInstructionMatch.Groups[1].ToString();
                                string coordsString = drawingInstructionMatch.Groups[2].ToString();
                                coordinates = coordsString.Split(new[] { ',' });
                                char firstCharacter = drawingInstruction[0];
                                switch (firstCharacter)
                                {
                                    case 'M':
                                        startX = Convert.ToDouble(coordinates[0], CultureInfo.InvariantCulture);
                                        startY = Convert.ToDouble(coordinates[1], CultureInfo.InvariantCulture);
                                        break;

                                    case 'C':
                                        controlPoint1X = Convert.ToSingle(coordinates[0], CultureInfo.InvariantCulture);
                                        controlPoint1Y = Convert.ToDouble(coordinates[1], CultureInfo.InvariantCulture);
                                        controlPoint2X = Convert.ToDouble(coordinates[2], CultureInfo.InvariantCulture);
                                        controlPoint2Y = Convert.ToDouble(coordinates[3], CultureInfo.InvariantCulture);
                                        endX = Convert.ToDouble(coordinates[4], CultureInfo.InvariantCulture);
                                        endY = Convert.ToDouble(coordinates[5], CultureInfo.InvariantCulture);
                                        graphics.DrawBezier(new Pen(Brushes.Black),
                                            (float)startX, (float)startY,
                                            (float)controlPoint1X, (float)controlPoint1Y,
                                            (float)controlPoint2X, (float)controlPoint2Y,
                                            (float)endX, (float)endY);

                                        startX = endX;
                                        startY = endY;
                                        break;

                                    case 'L':
                                        endX = Convert.ToDouble(coordinates[0], CultureInfo.InvariantCulture);
                                        endY = Convert.ToDouble(coordinates[1], CultureInfo.InvariantCulture);
                                        graphics.DrawLine(new Pen(Brushes.Black), (float)startX, (float)startY, (float)endX, (float)endY);
                                        startX = endX;
                                        startY = endY;
                                        break;
                                }
                            }
                        }
                    }

                    outputImageStream = new MemoryStream();
                    image.Save(outputImageStream, ImageFormat.Png);
                }
            }
            return outputImageStream;
        }
    }
}