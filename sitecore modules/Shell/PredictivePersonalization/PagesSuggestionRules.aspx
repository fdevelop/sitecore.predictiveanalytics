<%@ Page Title="" Language="C#" AutoEventWireup="true" 
  CodeBehind="PagesSuggestionRules.aspx.cs" Inherits="Sitecore.PredictivePersonalization.View.PagesSuggestionRules" %>

<!doctype>
<head>
    <title>TinyAnalytics :: Patterns :: PagesSuggestionRules</title>
    <style type="text/css">
      td {
        vertical-align: top;
      }
    </style>
</head>

<body>
  <form id="form1" runat="server">
    <h2>
        TinyAnalytics :: PagesSuggestionRules
    </h2>
    
    <div class="block-line">
        <div class="section">
            There are the associative rules generated according to pages viewed during the visits.
        </div>
        <div style="clear: both;">&nbsp;</div>
    </div>

    <div class="block-line">
        <div class="section">
          <asp:GridView ID="gridRules" runat="server">
          </asp:GridView>
        </div>
        <div style="clear: both;">&nbsp;</div>
    </div>
  </form>
</body>
