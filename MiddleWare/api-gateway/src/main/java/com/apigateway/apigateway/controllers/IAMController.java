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
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.reactive.function.client.WebClient;

import models.LoginRequest;
import models.User;

@RestController
public class IAMController {
	private final RestTemplate restTemplate;
    private static final Logger log = LoggerFactory.getLogger(IAMController.class);
    
    public IAMController(RestTemplateBuilder restTemplateBuilder) {
    	this.restTemplate = restTemplateBuilder.build();
    }
    
    @Autowired
    private String apiUrl;
    
    @Async
    @CrossOrigin(origins = "http://localhost:4200")
    @PostMapping("/login")
    public CompletableFuture<Object> loginPost(@RequestBody LoginRequest loginRequest) throws Exception {
		String url = apiUrl + "/v1/iam/login";
        String results = restTemplate.postForObject(url, loginRequest, String.class);
        return CompletableFuture.completedFuture(results);
    }
    
    @Async
    @CrossOrigin(origins = "http://localhost:4200")
    @PostMapping("/createuser")
    public CompletableFuture<Object> createUser(@RequestBody User user) throws Exception {
		String url = apiUrl + "/v1/iam/createuser";
        String results = restTemplate.postForObject(url, user, String.class);
        return CompletableFuture.completedFuture(results);
    }
    
    @Async
    @CrossOrigin(origins = "http://localhost:4200")
    @GetMapping("/isauthenticated")
    public CompletableFuture<Object> isAuthenticated(@RequestHeader HttpHeaders headers) {
		HttpEntity request = new HttpEntity(headers);
    	String url = apiUrl + "/v1/iam/isAuthenticated";
    	ResponseEntity<String> response = restTemplate.exchange(url, HttpMethod.GET, request, String.class);
        return CompletableFuture.completedFuture(response);
    }
}
