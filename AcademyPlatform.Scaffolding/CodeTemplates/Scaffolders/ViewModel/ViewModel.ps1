[T4Scaffolding.Scaffolder(Description = "Enter a description of ViewModel here")][CmdletBinding()]
param(    
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 0)][string]$ViewModelName,    
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, Position = 1)][string]$ModelType,
    [string]$Project,
    [string]$ModelsProject = ($project.Substring(0,$project.IndexOf(".")) + ".Models"),
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false,
	[string]$OutputFolder,
	[switch]$IncludeId = $false
)


	

$foundModelType = Get-ProjectType $ModelType -Project $ModelsProject -ErrorAction SilentlyContinue
$relatedEntities = [Array](Get-RelatedEntities $foundModelType.FullName -Project $modelsProject)
if (!$relatedEntities) { $relatedEntities = @() }

$namespace = [T4Scaffolding.Namespaces]::GetNamespace($foundModelType.FullName).Replace(".Models",".Web.Models");
$viewModelsProject = $ModelsProject.Replace(".Models",".Web.Models")
if(!($OutputFolder))
{
	$outputPath =   $namespace.Replace("AcademyPlatform.Web.Models.","")
}
else
{
	$outputPath =   $OutputFolder 

}

if($IncludeId)
{
	$included=$true;
}
else
{
	$included = $false;
}

write-host $included

$outputPath = Join-Path $outputPath $ViewModelName

Add-ProjectItemViaTemplate $outputPath -Template ViewModel `
	-Model @{ 
	Namespace = $namespace; 
	ViewModelName = $ViewModelName;
	ModelType = [MarshalByRefObject]$foundModelType; 
	IncludeId = $included;
	RelatedEntities = $relatedEntities;

	} `
	-SuccessMessage "Added ViewModel output at {0}" `
	-TemplateFolders $TemplateFolders -Project $viewModelsProject -CodeLanguage $CodeLanguage -Force:$Force