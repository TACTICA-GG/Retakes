using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Menus;
using SwiftlyS2.Core.Menus.OptionsBase;

namespace SwiftlyS2_Retakes.Utils;

/// <summary>
/// SwiftlyS2 themed image-menu helpers. Renders SVG button icons in the
/// CS2 center-HTML panel (footer in the comment slot, select icon on the
/// highlighted row). Single source of truth for menu styling.
/// </summary>
public static class ThemedMenu
{
  // Base URL of the SVG button icons (Pisex cs2-menus assets).
  private const string MenuBtnUrl =
    "https://raw.githubusercontent.com/Pisex/cs2-menus/refs/heads/main/menu_buttons/site";

  // One <img> tag. Native SVG size (no width/height).
  private static string Img(string name) => $"<img src='{MenuBtnUrl}/{name}.svg'/>";

  // The image "footer" row (goes in the comment slot).
  private static readonly string FooterButtons =
    $"{Img("w")} {Img("s")} {Img("empty")} {Img("f")}";

  // Select icon appended only to the row the player is currently navigating.
  private static readonly string SelectIcon = " " + Img("e");

  /// <summary>Creates a themed builder (single source of truth for styling).</summary>
  public static IMenuBuilderAPI CreateBuilder(ISwiftlyCore core, string title)
  {
    var builder = core.MenusAPI.CreateBuilder();
    var design = builder.Design;
    design.SetMenuTitle(title);
    design.SetMenuTitleVisible(true);          // title + auto line under it
    design.SetMenuTitleItemCountVisible(true); // "[2/7]" counter
    design.SetMenuFooterVisible(false);        // hide built-in TEXT footer
    design.SetCommentVisible(true);            // comment slot holds image buttons
    design.SetDefaultComment(FooterButtons);   // SVG buttons rendered as <img>
    design.SetNavigationMarkerColor("#FFFFFF");// white cursor
    design.SetVisualGuideLineColor("#FFFFFF"); // white guide lines
    design.SetDisabledColor("#808080");        // grey disabled options
    design.SetMaxVisibleItems(3);              // rows per page
    builder.EnableSound();
    return builder;
  }

  /// <summary>Appends the select icon only to the row the player is navigating.</summary>
  public static T Selectable<T>(ISwiftlyCore core, T opt) where T : MenuOptionBase
  {
    opt.AfterFormat += (_, args) =>
    {
      var menu = core.MenusAPI.GetCurrentMenu(args.Player);
      if (menu != null && ReferenceEquals(menu.GetCurrentOption(args.Player), args.Option))
        args.CustomText += SelectIcon;
    };
    return opt;
  }
}
