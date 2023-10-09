using System;

namespace CompanionCollector.Attributes;
[AttributeUsage(AttributeTargets.Method)]
public class DoNotShowInHelpAttribute : Attribute
{
}