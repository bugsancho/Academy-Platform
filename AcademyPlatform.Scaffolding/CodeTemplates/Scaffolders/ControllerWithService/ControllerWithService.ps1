[T4Scaffolding.ControllerScaffolder("Controller with read/write action and views, using EF data access code", Description = "Adds an ASP.NET MVC controller with views and data access code", SupportsModelType = $true, SupportsDataContextType = $true, SupportsViewScaffolder = $true)][CmdletBinding()]
param(     
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ControllerName,   
	[string]$ModelType,
    [string]$Project,
    [string]$CodeLanguage,
	[string]$DbContextType,
	[string]$Area,
	[string]$ViewScaffolder = "View",
	[alias("MasterPage")]$Layout,
 	[alias("ContentPlaceholderIDs")][string[]]$SectionNames,
	[alias("PrimaryContentPlaceholderID")][string]$PrimarySectionName,
	[switch]$ReferenceScriptLibraries = $false,
	[switch]$Service = $true,
	[switch]$NoChildItems = $false,
	[string[]]$TemplateFolders,
	[switch]$Force = $false,
	[string]$ForceMode
)

# Interpret the "Force" and "ForceMode" options
$overwriteController = $Force -and ((!$ForceMode) -or ($ForceMode -eq "ControllerOnly"))
$overwriteFilesExceptController = $Force -and ((!$ForceMode) -or ($ForceMode -eq "PreserveController"))

$baseProject = $project.Substring(0,$project.IndexOf("."))
$webProject = $baseProject + ".Web"
$modelsProject = $baseProject + ".Models"
$viewModelsProject = $baseProject + ".Web.Models"
$dataProject = $baseProject + ".Data"
$servicesProject = $baseProject + ".Services"

 #If you haven't specified a model type, we'll guess from the controller name
if (!$ModelType) {

	if ($ControllerName.EndsWith("Controller", [StringComparison]::OrdinalIgnoreCase)) {
		# If you've given "PeopleController" as the full controller name, we're looking for a model called People or Person
		$ModelType = [System.Text.RegularExpressions.Regex]::Replace($ControllerName, "Controller$", "", [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
		$foundModelType = Get-ProjectType $ModelType -Project $modelsProject -ErrorAction SilentlyContinue
		$foundViewModelType = Get-ProjectType ($ModelType + "EditViewModel") -Project $viewModelsProject -ErrorAction SilentlyContinue
		
		if (!$foundModelType) {
			$ModelType = [string](Get-SingularizedWord $ModelType)
			$foundModelType = Get-ProjectType $ModelType -Project $modelsProject -ErrorAction SilentlyContinue
			$foundViewModelType = Get-ProjectType ($ModelType + "EditViewModel") -Project $viewModelsProject -ErrorAction SilentlyContinue
		}
	} else {
		# If you've given "people" as the controller name, we're looking for a model called People or Person, and the controller will be PeopleController
		$ModelType = $ControllerName
		$foundModelType = Get-ProjectType $ModelType -Project $modelsProject -ErrorAction SilentlyContinue
		$foundViewModelType = Get-ProjectType ($ModelType + "EditViewModel") -Project $viewModelsProject -ErrorAction SilentlyContinue
		if (!$foundModelType) {
			$ModelType = [string](Get-SingularizedWord $ModelType)
			$foundModelType = Get-ProjectType $ModelType -Project $modelsProject -ErrorAction SilentlyContinue
			$foundViewModelType = Get-ProjectType ($ModelType + "EditViewModel") -Project $viewModelsProject -ErrorAction SilentlyContinue
		}
		if ($foundModelType) {
			$ControllerName = [string](Get-PluralizedWord $foundModelType.Name) + "Controller"
		}
	}
	if (!$foundModelType) { throw "Cannot find a model type corresponding to a controller called '$ControllerName'. Try supplying a -ModelType parameter value." }
} else {
	# If you have specified a model type
	$foundModelType = Get-ProjectType $ModelType -Project $modelsProject -ErrorAction SilentlyContinue
	$foundViewModelType = Get-ProjectType ($ModelType + "EditViewModel") -Project $viewModelsProject -ErrorAction SilentlyContinue
	if (!$foundModelType) { return }
	if (!$ControllerName.EndsWith("Controller", [StringComparison]::OrdinalIgnoreCase)) {
		$ControllerName = $ControllerName + "Controller"
	}
}

if(!$foundViewModelType)
{
	Scaffold ViewModel ($foundModelType.Name + "EditViewModel") $foundModelType.Name 
	Scaffold ViewModel ($foundModelType.Name + "ListViewModel") $foundModelType.Name -IncludeId
}

$foundViewModelType = Get-ProjectType ($ModelType + "EditViewModel") -Project $viewModelsProject -ErrorAction SilentlyContinue

#Write-Host "Scaffolding $ControllerName..."

if(!$DbContextType) { $DbContextType = [System.Text.RegularExpressions.Regex]::Replace($baseProject, "[^a-zA-Z0-9]", "") + "DbContext" }
if (!$NoChildItems) {
	if ($Service) {
		Scaffold Service -ModelType $foundModelType.FullName -Project $Project -OutputProject $servicesProject -CodeLanguage $CodeLanguage -Force:$overwriteFilesExceptController
	} else {
		$dbContextScaffolderResult = Scaffold DbContext -ModelType $foundModelType.FullName -DbContextType $DbContextType -Project $Project -OutputProject $dataProject -CodeLanguage $CodeLanguage
		$foundDbContextType = $dbContextScaffolderResult.DbContextType
		if (!$foundDbContextType) { return }
	}
}
if (!$foundDbContextType) { $foundDbContextType = Get-ProjectType $DbContextType -Project $dataProject }
if (!$foundDbContextType) { return }

$primaryKey = Get-PrimaryKey $foundModelType.FullName -Project $modelsProject -ErrorIfNotFound
if (!$primaryKey) { return }

$outputPath = Join-Path Controllers $ControllerName
# We don't create areas here, so just ensure that if you specify one, it already exists
if ($Area) {
	$areaPath = Join-Path Areas $Area
	if (-not (Get-ProjectItem $areaPath -Project $webProject)) {
		Write-Error "Cannot find area '$Area'. Make sure it exists already."
		return
	}
	$outputPath = Join-Path $areaPath $outputPath
}

# Prepare all the parameter values to pass to the template, then invoke the template with those values
$serviceName = $foundModelType.Name + "Service"
$defaultNamespace = (Get-Project $webProject).Properties.Item("DefaultNamespace").Value
$modelTypeNamespace = [T4Scaffolding.Namespaces]::GetNamespace($foundModelType.FullName)
$viewModelNamespace = [T4Scaffolding.Namespaces]::GetNamespace($foundViewModelType.FullName)
$controllerNamespace = [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + "." + [System.IO.Path]::GetDirectoryName($outputPath).Replace([System.IO.Path]::DirectorySeparatorChar, "."))
$areaNamespace = if ($Area) { [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + ".Areas.$Area") } else { $defaultNamespace }
$dbContextNamespace = $foundDbContextType.Namespace.FullName
$servicesNamespace = [T4Scaffolding.Namespaces]::Normalize($servicesProject + ".Contracts")
$modelTypePluralized = Get-PluralizedWord $foundModelType.Name
$relatedEntities = [Array](Get-RelatedEntities $foundModelType.FullName -Project $modelsProject)
if (!$relatedEntities) { $relatedEntities = @() }


Add-ProjectItemViaTemplate $outputPath -Template "ControllerWithService" -Model @{
	ControllerName = $ControllerName;
	ModelType = [MarshalByRefObject]$foundModelType; 
	ViewModelType = [MarshalByRefObject]$foundViewModelType; 
	PrimaryKey = [string]$primaryKey; 
	DefaultNamespace = $defaultNamespace; 
	AreaNamespace = $areaNamespace; 
	DbContextNamespace = $dbContextNamespace;
	ServicesNamespace = $servicesNamespace;
	ViewModelNamespace = $viewModelNamespace;
	ModelTypeNamespace = $modelTypeNamespace; 
	ControllerNamespace = $controllerNamespace; 
	DbContextType = [MarshalByRefObject]$foundDbContextType;
	Service = $serviceName; 
	ModelTypePluralized = [string]$modelTypePluralized; 
	RelatedEntities = $relatedEntities;
} -SuccessMessage "Added controller {0}" -TemplateFolders $TemplateFolders -Project $webProject -CodeLanguage $CodeLanguage -Force:$overwriteController

if (!$NoChildItems) {
	$controllerNameWithoutSuffix = [System.Text.RegularExpressions.Regex]::Replace($ControllerName, "Controller$", "", [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
	if ($ViewScaffolder) {
		Scaffold Views -ViewScaffolder $ViewScaffolder -Controller $controllerNameWithoutSuffix -ModelType $foundModelType.Name -Area $Area -Layout $Layout -SectionNames $SectionNames -PrimarySectionName $PrimarySectionName -ReferenceScriptLibraries:$ReferenceScriptLibraries -Project $Project -OutputProject $webProject -CodeLanguage $CodeLanguage -Force:$overwriteFilesExceptController
	}
}