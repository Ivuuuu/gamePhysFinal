/// Manages the cleaning of events and other things, imported from a class in Roblox.
/// Useful for encapsulating state and make deconstructors easy
/// @author  Ryan Vo
/// @version 1.0
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
/// <summary>
/// Manages the cleaning of events and other things.
/// </summary>
/// <remarks>
/// Imported from a class in Roblox, originally made by Quenty.
/// <para>
/// In Roblox/Lua, this was able to use promises and could be used as an array, but I haven't implemented those yet.
/// </para>
/// </remarks>
public class Maid : IDisposable {
	private static readonly Maid GlobalMaid = new Maid();
	public static void KillAll() {
		GlobalMaid.Dispose();
	}

	public List<object> _tasks = new List<object>();

	/// <summary>
	/// Constructs a Maid class and is automatically stored inside a Global Maid, which will automatically destroy itself when the application quits.
	/// </summary>
	public Maid() {
		GlobalMaid?.GiveTask(this);
	}

	/// <summary>
	/// Add a task to clean up.
	/// </summary>
	/// <param name="task">The task being added. Good practice for it to be an IDisposable</param>
	/// <returns>An unique integer defining the ID of the task.</returns>
	public int GiveTask(object task) {
		int taskId = this._tasks.Count;
		this._tasks.Add(task);
		return taskId;
	}

	///<summary>Cleans up all tasks.</summary>
	public void DoCleaning() {
		int i;
		//Disconnect all events first as we know this is safe
		for (i = 0; i < this._tasks.Count; i++) {
			if (this._tasks[i].GetType() == typeof(UnityEvent)) {
				((UnityEvent)this._tasks[i]).RemoveAllListeners();
				this._tasks[i] = null;
			}
		}

		//Clear out tasks table completely, even if clean up tasks add more tasks to the maid
		i = 0;
		while (i < this._tasks.Count) {
			object task = this._tasks[i];
			if (task != null) {
				this._tasks[i] = null;
				if (task is IDisposable) {	
					((IDisposable)task).Dispose();
				} else if (task is GameObject) {
					GameObject.Destroy((GameObject)task);
				} else if (task is Action) {
					((Action)task)();
				}
				i++;
			}
		}
		this._tasks.Clear();
	}

	///<summary>Same as .DoCleaning()</summary>
	public void Dispose() {
		this.DoCleaning();
	}
}

/*
---	Manages the cleaning of events and other things.
-- Useful for encapsulating state and make deconstructors easy
-- @classmod Maid
-- @see Signal

local Maid = {}
Maid.ClassName = "Maid"

--- Returns a new Maid object
-- @constructor Maid.new()
-- @treturn Maid
function Maid.new()
	local self = {}

	self._tasks = {}

	return setmetatable(self, Maid)
end

--- Returns Maid[key] if not part of Maid metatable
-- @return Maid[key] value
function Maid:__index(index)
	if Maid[index] then
		return Maid[index]
	else
		return self._tasks[index]
	end
end

--- Add a task to clean up
-- @usage
-- Maid[key] = (function)         Adds a task to perform
-- Maid[key] = (event connection) Manages an event connection
-- Maid[key] = (Maid)             Maids can act as an event connection, allowing a Maid to have other maids to clean up.
-- Maid[key] = (object)           Maids can cleanup objects with a `Destroy` method
-- Maid[key] = nil                Removes a named task. If the task is an event, it is disconnected. If it is an object, it is destroyed.
function Maid:__newindex(index, newTask)
	if Maid[index] ~= nil then
		error(("'%s' is reserved"):format(tostring(index)), 2)
	end

	local tasks = self._tasks
	local oldTask = tasks[index]
	tasks[index] = newTask

	if oldTask then
		if type(oldTask) == "function" then
			oldTask()
		elseif typeof(oldTask) == "RBXScriptConnection" then
			oldTask:Disconnect()
		elseif oldTask.Destroy then
			oldTask:Destroy()
		end
	end
end

--- Same as indexing, but uses an incremented number as a key.
-- @param task An item to clean
-- @treturn number taskId
function Maid:GiveTask(task)
	assert(task)
	local taskId = #self._tasks+1
	self[taskId] = task

	if type(task) == "table" and (not task.Destroy) then
		warn("[Maid.GiveTask] - Gave table task without .Destroy\n\n" .. debug.traceback())
	end

	return taskId
end

function Maid:GivePromise(promise)
	if not promise:IsPending() then
		return promise
	end

	local newPromise = promise.resolved(promise)
	local id = self:GiveTask(newPromise)

	-- Ensure GC
	newPromise:Finally(function()
		self[id] = nil
	end)

	return newPromise
end

--- Cleans up all tasks.
-- @alias Destroy
function Maid:DoCleaning()
	local tasks = self._tasks

	-- Disconnect all events first as we know this is safe
	for index, task in pairs(tasks) do
		if typeof(task) == "RBXScriptConnection" then
			tasks[index] = nil
			task:Disconnect()
		end
	end

	-- Clear out tasks table completely, even if clean up tasks add more tasks to the maid
	local index, task = next(tasks)
	while task ~= nil do
		tasks[index] = nil
		if type(task) == "function" then
			task()
		elseif typeof(task) == "RBXScriptConnection" then
			task:Disconnect()
		elseif task.Destroy then
			task:Destroy()
		end
		index, task = next(tasks)
	end
end

--- Alias for DoCleaning()
-- @function Destroy
Maid.Destroy = Maid.DoCleaning

return Maid
*/