import axios from 'axios';

//process.env.REACT_APP_API_URL
axios.defaults.baseURL = "http://localhost:5272/items"

axios.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    console.error('API Error:', error.response ? error.response.data : error.message);
    return Promise.reject(error);
  }
);

export default {
  getTasks: async () => {
    try {
      const result = await axios.get("/")
      return result.data;
    }
    catch (error) {
      console.error(error);
      throw error;
    }
  },

  addTask: async (name) => {
    try {
      console.log('addTask', name)
      const result = await axios.post("/", { name: name, isComplete: false }); // שולח בקשה להוספת משימה
      return result.data;
    }
    catch (error) {
      console.error(error);
      throw error;
    }
  },

  setCompleted: async (id, isComplete) => {
    try {
      console.log('setCompleted', { id, isComplete })
      const result = await axios.put(`/${id}`, { isComplete: isComplete });
      return result.data;
    }
    catch (error) {
      console.error(error);
      throw error;
    }
  },

  deleteTask: async (event) => {
    try {
      console.log('deleteTask')
      const result = await axios.delete(`/${event}`);
      return result.data;
    }
    catch (error) {
      console.error(error);
      throw error;
    }
  }
};