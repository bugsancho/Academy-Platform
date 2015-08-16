[T4Scaffolding.ViewScaffolder("Razor", Description = "Adds an ASP.NET MVC view using the Razor view engine", IsRazorType = $true, LayoutPageFilter = "*.cshtml|*.cshtml")][CmdletBinding()]
param(        
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$Controller,
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 1)][string]$ViewName,
	[string]$ModelType,
	[string]$Template = "Empty",
	[string]$Area,
	[alias("MasterPage")]$Layout,	# If not set, we'll use the default layout
 	[string[]]$SectionNames,
	[string]$PrimarySectionName,
	[switch]$ReferenceScriptLibraries = $false,
    [string]$Project,
	[string]$OutputProject,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)
$baseProject = $project.Substring(0,$project.IndexOf("."))
$modelsProject = $baseProject + ".Models"
$viewModelsProject = $baseProject + ".Web.Models"



# Ensure we have a controller name, plus a model type if specified
if ($ModelType) {
	$foundModelType = Get-ProjectType $ModelType -Project $modelsProject
	$foundEditViewModelType = Get-ProjectType ($ModelType + "EditViewModel") -Project $viewModelsProject
	$foundListViewModelType = Get-ProjectType ($ModelType + "ListViewModel") -Project $viewModelsProject
	if (!$foundModelType) { return }
	$primaryKeyName = [string](Get-PrimaryKey $foundModelType.FullName -Project $modelsProject)
}

# Decide where to put the output
$outputFolderName = Join-Path Views $Controller
if ($Area) {
	# We don't create areas here, so just ensure that if you specify one, it already exists
	$areaPath = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaPath -Project $OutputProject)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	$outputFolderName = Join-Path $areaPath $outputFolderName
}


if ($foundModelType) { $relatedEntities = [Array](Get-RelatedEntities $foundModelType.FullName -Project $modelsProject) }
if (!$relatedEntities) { $relatedEntities = @() }

# Render the T4 template, adding the output to the Visual Studio project
$outputPath = Join-Path $outputFolderName $ViewName
$pluralName = Get-PluralizedWord $foundModelType.Name

Add-ProjectItemViaTemplate $outputPath -Template $Template -Model @{
	IsContentPage = [bool]$Layout;
	Layout = $Layout;
	SectionNames = $SectionNames;
	PrimarySectionName = $PrimarySectionName;
	ReferenceScriptLibraries = $ReferenceScriptLibraries.ToBool();
	ViewName = $ViewName;
	Area = $Area;
	PrimaryKeyName = $primaryKeyName;
	PluralName = $pluralName;
	ViewDataType = [MarshalByRefObject]$foundModelType;
	ListViewModelDataType = [MarshalByRefObject]$foundListViewModelType;
	EditViewModelDataType = [MarshalByRefObject]$foundEditViewModelType;
	ViewDataTypeName = $foundModelType.Name;
	RelatedEntities = $relatedEntities;
} -SuccessMessage "Added $ViewName view at '{0}'" -TemplateFolders $TemplateFolders -Project $OutputProject -CodeLanguage $CodeLanguage -Force:$Force
