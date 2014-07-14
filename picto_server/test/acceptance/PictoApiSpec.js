var request = require('supertest');

describe('picto api', function(){

	describe('get categories', function(){
		it('should return all categories for a default language', function(done){
			var result;

			request('http://localhost:3000')
				.get('/categories')
				.end(function(err, res){
					if(err){
						throw err;
					}
					result=res;
				});

			expect(result.status).toEqual(200);
			//expect(result.body.)
			done();
		});

		xit('should return all categories for a given language', function(){
			
		});
	});

	describe('get category\'s pictos', function(){
		xit('should return all category\'s pictos for a default language', function(){

		});
	});

	describe('get category\'s pictos by language', function(){
		xit('should return all category\'s pictos for given language', function(){
			//get(/abstract)
		});
	});

	describe('get category image', function(){
		xit('should return image', function(){
			//get(/image/abstract)
			//get(/image/abstract/hi)
		});
	});

	describe('get picto image', function(){
		xit('should return image', function(){
			//get(/image/abstract)
			//get(/image/abstract/hi)
		});
	});
})