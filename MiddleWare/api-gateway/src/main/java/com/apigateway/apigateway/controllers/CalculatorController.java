package com.apigateway.apigateway.controllers;

import java.util.concurrent.CompletableFuture;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.web.client.RestTemplateBuilder;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.scheduling.annotation.Async;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

import models.LoginRequest;
import models.NetworthRequest;
import models.User;

@RestController
public class CalculatorController {
	private final RestTemplate restTemplate;
    private static final Logger log = LoggerFactory.getLogger(CalculatorController.class);
    
    public CalculatorController(RestTemplateBuilder restTemplateBuilder) {
    	this.restTemplate = restTemplateBuilder.build();
    }
    
    @Autowired
    private String apiUrl;
    
    @Async
    @CrossOrigin(origins = "http://localhost:4200")
    @PostMapping("/calculatenetworth")
    public CompletableFuture<String> calculateNetworth(@RequestBody NetworthRequest networthRequest, @RequestHeader HttpHeaders headers) {
		/*
		 * headers.forEach((key, value) -> { LOG.info(String.format( "Header '%s' = %s",
		 * key, value.stream().collect(Collectors.joining("|")))); });
		 */
    	
    	HttpEntity<NetworthRequest> request = new HttpEntity<>(networthRequest, headers);
        //ResponseEntity<User> response = restTemplate.exchange(url, HttpMethod.GET, request, User.class);
    	
    	String url = apiUrl + "/v1/calculator/calculatenetworth";
        String results = restTemplate.postForObject(url, request, String.class);
        return CompletableFuture.completedFuture(results);
    }
}
