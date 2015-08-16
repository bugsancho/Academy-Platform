[T4Scaffolding.Scaffolder(Description = "Creates a neat service")][CmdletBinding()]
param(        
    [parameter(Position = 0, Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
    [string]$Project,
    [string]$OutputProject,
    [string]$CodeLanguage,
	[switch]$NoChildItems = $false,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

# Ensure you've referenced System.Data.Entity
(Get-Project $Project).Object.References.Add("System.Data.Entity") | Out-Null


$baseProject = $project.Substring(0,$project.IndexOf("."))
$modelsProject = $baseProject + ".Models"

$foundModelType = Get-ProjectType $ModelType -Project $modelsProject
if (!$foundModelType) { return }

$primaryKey = Get-PrimaryKey $foundModelType.FullName -Project $modelsProject -ErrorIfNotFound
if (!$primaryKey) { return }

		
$outputPath =$foundModelType.Name + "Service"
$contractOutputPath  =  Join-Path Contracts  ("I"+ $outputPath)

$baseProject = $project.Substring(0,$project.IndexOf("."))
#$dataProjectFolder = Join-Path ($baseProject + ".Data") Repository
$modelTypePluralized = Get-PluralizedWord $foundModelType.Name
$defaultNamespace = (Get-Project $OutputProject).Properties.Item("DefaultNamespace").Value
$serviceNamespace = [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + "." + [System.IO.Path]::GetDirectoryName($outputPath).Replace([System.IO.Path]::DirectorySeparatorChar, "."))
$serviceContractNamespace = [T4Scaffolding.Namespaces]::Normalize($defaultNamespace + "." + [System.IO.Path]::GetDirectoryName($contractOutputPath).Replace([System.IO.Path]::DirectorySeparatorChar, "."))
$repositoryNamespace = [T4Scaffolding.Namespaces]::Normalize($baseProject+ ".Data.Repositories")
$modelTypeNamespace = [T4Scaffolding.Namespaces]::GetNamespace($foundModelType.FullName)


Add-ProjectItemViaTemplate $outputPath -Template Service -Model @{
	ModelType = [MarshalByRefObject]$foundModelType; 
	PrimaryKey = [string]$primaryKey; 
	DefaultNamespace = $defaultNamespace; 
	ServiceNamespace = $serviceNamespace; 
	ServiceContractNamespace = $serviceContractNamespace;
	RepositoryNamespace = $repositoryNamespace;
	ModelTypeNamespace = $modelTypeNamespace; 
	ModelTypePluralized = [string]$modelTypePluralized; 
} -SuccessMessage "Added service '{0}'" -TemplateFolders $TemplateFolders -Project $OutputProject -CodeLanguage $CodeLanguage -Force:$Force

Add-ProjectItemViaTemplate $contractOutputPath -Template ServiceContract -Model @{
	ModelType = [MarshalByRefObject]$foundModelType; 
	PrimaryKey = [string]$primaryKey; 
	DefaultNamespace = $defaultNamespace; 
	ServiceNamespace = $serviceContractNamespace; 
	ModelTypeNamespace = $modelTypeNamespace; 
	ModelTypePluralized = [string]$modelTypePluralized; 
} -SuccessMessage "Added service contract '{0}'" -TemplateFolders $TemplateFolders -Project $OutputProject -CodeLanguage $CodeLanguage -Force:$Force

