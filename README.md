# dotnet-opentelemetry-sample


## Pre-requisite
Make sure you have docker installed on your machine.

## Setup

To run the sample. Navigate to the scripts folder and execute the `Start.ps1` script

or if you'd like to start the services manually, you can run
```
docker-compose up -d  --build --force-recreate --remove-orphans
```

Once all the services are up and running, you can navigate to [http://localhost:7100](http://localhost:7100) to open the web app. 

The web application sends a request to FirstApi hosted at [http://localhost:7000](http://localhost:7000) which then makes few HTTP requests to SecondApi hosted at [http://localhost:7001](http://localhost:7001).

If everything is setup correctly, you should see trace logged in Jaeger ([http://localhost:16686/](http://localhost:16686/))

![Jaeger UI](https://github.com/vivekchari/dotnet-opentelemetry-sample/blob/master/images/jaeger.png)

The web application and Apis are setup to export metrics. You can view the metrics exposed by individual services by navigating to the `/metrics` path for e.g. [http://localhost:7100/metrics](http://localhost:7100/metrics).

![Prometheus](https://github.com/vivekchari/dotnet-opentelemetry-sample/blob/master/images/prometheus.png)

The [config](https://github.com/vivekchari/dotnet-opentelemetry-sample/blob/master/data/prometheus/prometheus.yml) file with the sample app configures Prometheus to scrape metrics from these three services. You can view the metrics directly in Prometheus ([http://localhost:9090](http://localhost:9090)) or use the Grafana instance started by the sample app:

- Navigate to http://localhost:3000 to open Grafana.
- Setup a password (if you haven't already)
- Add a new Prometheus datasource. <br>
  **Note**: If you're using the Grafana instance setup by the sample app, the URL to the datasource will need to be the service name defined in the docker compose file i.e. `http://prometheus-host:9090/`
- Import [this](https://grafana.com/grafana/dashboards/10915) dashboard which displays the ASP.NET controller summary. The links also has details on how to import the dashboard in Grafana.

Once you've imported the dashboard, refresh the web app few times and you should see the data in the dashboard:

![Grafana Dashboard](https://github.com/vivekchari/dotnet-opentelemetry-sample/blob/master/images/grafana.png)






