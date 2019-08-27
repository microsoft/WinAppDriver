## WinAppDriver in CI with Azure Pipelines

### Prerequisites to run WinAppDriver in CI
The following are prequisites for running UI tests with WinAppDriver in Azure DevOps:

 1. An agent running on Windows 10 configured as interactive is required.
        
    - The following hosted agents are supported: [HostedVS2019](https://github.com/Microsoft/azure-pipelines-image-generation/blob/master/images/win/Vs2019-Server2019-Readme.md) and [HostedVS2017](https://github.com/Microsoft/azure-pipelines-image-generation/blob/master/images/win/Vs2017-Server2016-Readme.md).
       - If you wish to use a private agent, please refer to the Azure documentation [here](https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/v2-windows?view=azure-devops) on setting one up.
    - If the hosted agents do not meet your requirement, try using a private agent. More information on this [below](https://github.com/Microsoft/WinAppDriver/wiki/Continuous-Integration-with-WinAppDriver/_edit#getting-started-with-a-sample-build-pipeline). 
 1. Use Azure Pipelines - [get started today for free](https://azure.microsoft.com/en-us/pricing/details/devops/azure-pipelines/).  

### WinAppDriver Task for Azure Pipelines 
There's now a dedicated [WinAppDriver Pipelines Task](https://marketplace.visualstudio.com/items?itemName=WinAppDriver.winappdriver-pipelines-task) available on the Azure Marketplace to help you easily enable and configure WinAppDriver from inside your DevOps Pipeline. To get started, install the WinAppDriver task onto your DevOps organization from [here](https://marketplace.visualstudio.com/acquisition?itemName=WinAppDriver.winappdriver-pipelines-task), or you can search through the **Add Tasks** menu from inside your Pipeline Editor - 

<p align="center"><img src="https://raw.githubusercontent.com/hassanuz/Sandbox/master/snippets/AzurePipelines/Task/downloadTask.PNG" width="700" align="middle"></p>

Once installed, the WinAppDriver task can be added to your pipeline. 

<p align="center"><img src="https://raw.githubusercontent.com/hassanuz/Sandbox/master/snippets/AzurePipelines/Task/pipelineAddTask.PNG" width="700" align="middle"></p>

It is recommended to place the WinAppDriver task in conjunction with a separate Test Runner utility - in this case our pipeline has been pre-equipped with the Visual Studio Test Runner Task to drive the test cases. 

<p align="center"><img src="https://raw.githubusercontent.com/hassanuz/Sandbox/master/snippets/AzurePipelines/Task/startTaskAdded.PNG" width="300" align="middle"></p>

We can now configure the task - you can see that automatically the Task is named to "Start - WinAppDriver". It is not required to pass in any arguments, but it is recommended to set the System Resolution on Agent to **1080P** - this will be especially important to declare on a hosted agent. 

<p align="center"><img src="https://raw.githubusercontent.com/hassanuz/Sandbox/master/snippets/AzurePipelines/Task/startTaskOptions.PNG" width="700" align="middle"></p>

Once configured, it's recommended to add another instance of the WinAppDriver task, this time having it be configured to "Stop" WinAppDriver on the agent. The "Stop- WinAppDriver" instance of the task should be placed after the VS Test task, so that WinAppDriver is terminated on the agent once all the test cases have been executed. 

<p align="center"><img src="https://raw.githubusercontent.com/hassanuz/Sandbox/master/snippets/AzurePipelines/Task/stopTaskOptions.PNG" width="700" align="middle"></p>

Congratulations! Assuming all went well, you can now view the captured WinAppDriver logs from the Summary Panel of the "Close - WinAppDriver" instance of the task. 

<p align="center"><img src="https://raw.githubusercontent.com/hassanuz/Sandbox/master/snippets/AzurePipelines/Task/ResultsSummary.PNG" width="700" align="middle"></p>

And the WinAppDriver logs,

<p align="center"><img src="https://raw.githubusercontent.com/hassanuz/Sandbox/master/snippets/AzurePipelines/Task/Results.PNG" width="700" align="middle"></p>

Finally after you've finished with all the steps above, don't forget to add the finishing touches to your repository by embedding the official [Azure Pipelines status badge](https://docs.microsoft.com/en-us/azure/devops/pipelines/create-first-pipeline?view=azure-devops&tabs=tfs-2018-2#add-a-status-badge-to-your-repository)! 
